/**
 * File: MuxGraph.cs
 * 
 *	Copyright © 2016-2017 by City Council Video.  All rights reserved.
 *
 *	$Id: /Video_Test_Fixture/MuxGraph.cs,v $
 */
/**
*	Implements the multiplexer graph for the VGV applications
*
*	Author:			Fred Koschara
*	Creation Date:	April seventh, 2017
*	Last Modified:	April 26, 2017 @ 11:45 am
*
*	Revision History:
*	   Date		  by		Description
*	2017/04/26	wfredk	original development
*		|						|
*	2017/04/07	wfredk	original development
*/
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using DirectShowLib;
using GMFBridgeLib;

using Utility.VgvUtility;

namespace Video_Test_Fixture
{
    /// <summary>
    /// Implements the multiplexer graph for the VGV applications   
    /// </summary>
    public class MuxGraph
    {
        private GlobalConfig cfg = null;
//      private IBaseFilter outFilter = null;
        private VgvBridge[] inBridge = null;
        private VgvBridge outBridge = null;

        private IBaseFilter[] bridgeSrcFilter = null;

        private IGraphBuilder graph = null;
        private IMediaControl mediaCtl = null;

        /// <summary>
        /// returns a handle to the filter graph
        /// 
        /// The filter graph is instantiated the first time this property
        /// is accessed.
        /// </summary>
        public IGraphBuilder Graph
        {   get
            {   if (graph == null)
                {   try
                    {   graph = (IGraphBuilder)new FilterGraph();
                        mediaCtl = (IMediaControl)graph;
                    }
                    catch (COMException ex)
                    {   MessageBox.Show("COM Error: " + ex.ToString());
                    }
                    catch (Exception ex)
                    {   MessageBox.Show("Error: " + ex.ToString());
                    }
                }
                return graph;
            }
        }
        /// <summary>
        /// Gets the media control.
        /// </summary>
        /// <value>
        /// The media control.
        /// </value>
        public IMediaControl MediaCtl
        {   get
            {   if (mediaCtl == null)
                {   try
                    {   IGraphBuilder theGraph = Graph;
                    }
                    catch (Exception ex)
                    {   MessageBox.Show("Error: " + ex.ToString());
                    }
                }
                return mediaCtl;
            }
        }

        // --------------------------------------------------------------------
        // constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MuxGraph"/> class.
        /// </summary>
        public MuxGraph()
        {
            cfg = GlobalConfig.Instance;
            int cnt;
            int hr;
            int nLim = cfg.NumCameras;

            inBridge = new VgvBridge[nLim];
            bridgeSrcFilter = new IBaseFilter[nLim];

            // required for mock implementation only
            Guid CLSID_NullRenderer = new Guid("{C1F400A4-3F08-11D3-9F0B-006008039E37}"); //qedit.dll

            for (cnt = 0; cnt < nLim; cnt++)
            {
                IBaseFilter pMuxInput;
                IBaseFilter sink;

                inBridge[cnt] = new VgvBridge();
                sink = cfg.Camera(cnt).connectSrcGraph(inBridge[cnt]);
                // start the source graph
                cfg.Camera(cnt).MediaCtl.Run();

                // ***********************
                // TODO: build multiplexor
                // ***********************
                {
                    // create null renderer
                    pMuxInput = (IBaseFilter)Activator.CreateInstance(Type.GetTypeFromCLSID(CLSID_NullRenderer));
                    hr = Graph.AddFilter(pMuxInput,"Null Renderer");
                    VgvUtil.checkHR(hr,"Can't add Null Renderer "+cnt+" to graph");
                }

                bridgeSrcFilter[cnt] = inBridge[cnt].ConnectOutput(VgvUtil.GetPin(pMuxInput,"In"),Graph);
            }

            // after mux is constructed, connect the source graphs and mux
            for (cnt = 0; cnt < nLim; cnt++)
                inBridge[cnt].ConnectGraphs();

            outBridge = new VgvBridge();
//          outBridge = new VgvBridge(Graph,VgvUtil.GetPin(pMuxFilter,"Output"));
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="MuxGraph"/> class.
        /// </summary>
        ~MuxGraph()
        {
/*
            int cnt;
            int nLim = cfg.NumCameras;

            for (cnt = 0; cnt < nLim; cnt++)    // disconnect source graphs
                inBridge[cnt].Bridge.BridgeGraphs(null,null);

            mediaCtl.Stop();    // stop this graph

            for (cnt = 0; cnt < nLim; cnt++)    // release resources
            {
                inBridge[cnt]=null;
                bridgeSrcFilter[cnt] = null;
            }
*/
        }
    }
}
//
// EOF: MuxGraph.cs
