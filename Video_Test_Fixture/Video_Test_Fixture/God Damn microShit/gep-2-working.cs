//Don't forget to add reference to DirectShowLib in your project.
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices;
using DirectShowLib;

namespace graphcode
{
    class Program
    {
        [System.Runtime.InteropServices.DllImport("OLE32.DLL",EntryPoint = "CreateStreamOnHGlobal")]
        extern public static int CreateStreamOnHGlobal(IntPtr ptr,bool delete,out IStream pOutStm);

        static byte[] filter_data = {
            0, 11, 0, 0, 8, 0, 136, 0, 0, 0, 104, 0, 116, 0, 116, 0,
            112, 0, 58, 0, 47, 0, 47, 0, 49, 0, 48, 0, 48, 0, 46, 0,
            48, 0, 46, 0, 49, 0, 57, 0, 51, 0, 46, 0, 57, 0, 56, 0,
            58, 0, 49, 0, 48, 0, 50, 0, 54, 0, 47, 0, 97, 0, 120, 0,
            105, 0, 115, 0, 45, 0, 99, 0, 103, 0, 105, 0, 47, 0, 109, 0,
            106, 0, 112, 0, 103, 0, 47, 0, 118, 0, 105, 0, 100, 0, 101, 0,
            111, 0, 46, 0, 99, 0, 103, 0, 105, 0, 63, 0, 114, 0, 101, 0,
            115, 0, 111, 0, 108, 0, 117, 0, 116, 0, 105, 0, 111, 0, 110, 0,
            61, 0, 54, 0, 52, 0, 48, 0, 120, 0, 51, 0, 54, 0, 48, 0,
            0, 0, 3, 0, 128, 2, 0, 0, 3, 0, 104, 1, 0, 0
        };

        static void checkHR(int hr, string msg)
        {
            if (hr < 0)
            {
                Console.WriteLine(msg);
                DsError.ThrowExceptionForHR(hr);
            }
        }

        static void BuildGraph(IGraphBuilder pGraph)
        {
            int hr = 0;

            //graph builder
            ICaptureGraphBuilder2 pBuilder = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            hr = pBuilder.SetFiltergraph(pGraph);
            checkHR(hr, "Can't SetFiltergraph");

            Guid CLSID_AlaxInfoJPEGVideoSource = new Guid("{A8DA2ECB-DEF6-414D-8CE2-E651640DBA4F}"); //IpVideoSource.dll
            Guid CLSID_VideoRenderer = new Guid("{B87BEB7B-8D29-423F-AE4D-6582C10175AC}"); //quartz.dll

            //add Alax.Info JPEG Video Source
            IBaseFilter pAlaxInfoJPEGVideoSource = (IBaseFilter)Activator.CreateInstance(Type.GetTypeFromCLSID(CLSID_AlaxInfoJPEGVideoSource));
            hr = pGraph.AddFilter(pAlaxInfoJPEGVideoSource, "Alax.Info JPEG Video Source");
            checkHR(hr, "Can't add Alax.Info JPEG Video Source to graph");

            //in your graph building code:
            IPersistStream ips = pAlaxInfoJPEGVideoSource as IPersistStream;
            if (ips != null)
            {
                IStream stream;
                IntPtr hg = Marshal.AllocHGlobal(filter_data.Length);
                Marshal.Copy(filter_data,0,hg,filter_data.Length);
                if (CreateStreamOnHGlobal(hg,false,out stream) == 0)
                    checkHR(ips.Load(stream),"Can't load filter state.");
                Marshal.FreeHGlobal(hg);
            }

            //add Video Renderer
            IBaseFilter pVideoRenderer = (IBaseFilter)Activator.CreateInstance(Type.GetTypeFromCLSID(CLSID_VideoRenderer));
            hr = pGraph.AddFilter(pVideoRenderer, "Video Renderer");
            checkHR(hr, "Can't add Video Renderer to graph");

            //connect Alax.Info JPEG Video Source and Video Renderer
            hr = pGraph.ConnectDirect(GetPin(pAlaxInfoJPEGVideoSource, "Output"), GetPin(pVideoRenderer, "VMR Input0"), null);
            checkHR(hr, "Can't connect Alax.Info JPEG Video Source and Video Renderer");

        }

        static void Main(string[] args)
        {
            try
            {
                IGraphBuilder graph = (IGraphBuilder)new FilterGraph();
                Console.WriteLine("Building graph...");
                BuildGraph(graph);
                Console.WriteLine("Running...");
                IMediaControl mediaControl = (IMediaControl)graph;
                IMediaEvent mediaEvent = (IMediaEvent)graph;
                int hr = mediaControl.Run();
                checkHR(hr, "Can't run the graph");
                bool stop = false;
                while (!stop)
                {
                    System.Threading.Thread.Sleep(500);
                    Console.Write(".");
                    EventCode ev;
                    IntPtr p1, p2;
                    System.Windows.Forms.Application.DoEvents();
                    while (mediaEvent.GetEvent(out ev, out p1, out p2, 0) == 0)
                    {
                        if (ev == EventCode.Complete || ev == EventCode.UserAbort)
                        {
                            Console.WriteLine("Done!");
                            stop = true;
                        }
                        else
                        if (ev == EventCode.ErrorAbort)
                        {
                            Console.WriteLine("An error occured: HRESULT={0:X}", p1);
                            mediaControl.Stop();
                            stop = true;
                        }
                        mediaEvent.FreeEventParams(ev, p1, p2);
                    }
                }
            }
            catch (COMException ex)
            {
                Console.WriteLine("COM error: " + ex.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.ToString());
            }
        }
        static IPin GetPin(IBaseFilter filter, string pinname)
        {
            IEnumPins epins;
            int hr = filter.EnumPins(out epins);
            checkHR(hr, "Can't enumerate pins");
            IntPtr fetched = Marshal.AllocCoTaskMem(4);
            IPin[] pins = new IPin[1];
            while (epins.Next(1, pins, fetched) == 0)
            {
                PinInfo pinfo;
                pins[0].QueryPinInfo(out pinfo);
                bool found = (pinfo.name == pinname);
                DsUtils.FreePinInfo(pinfo);
                if (found)
                    return pins[0];
            }
            checkHR(-1, "Pin not found");
            return null;
        }

    }
} 
