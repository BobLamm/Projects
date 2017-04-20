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
*	Last Modified:	April 13, 2017 @ 10:22 pm
*
*	Revision History:
*	   Date		  by		Description
*	2017/04/13	wfredk	original development
*		|						|
*	2017/04/07	wfredk	original development
*/
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using DirectShowLib;
using GMFBridgeLib;

namespace Video_Test_Fixture
{
    /// <summary>
    /// Implements the multiplexer graph for the VGV applications   
    /// </summary>
    public class MuxGraph
    {
        private GlobalConfig cfg = null;
        private VgvBridge[] inBridge = null;
        private VgvBridge outBridge = null;

        private IBaseFilter[] bridgeSrcFilter = null;

        IGraphBuilder graph = null;
        IMediaControl mediaCtl = null;
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
            int nLim = cfg.NumCameras;

            inBridge = new VgvBridge[nLim];
            bridgeSrcFilter = new IBaseFilter[] { };

            for (int cnt = 0; cnt < nLim; cnt++)
            {
                IBaseFilter sink = cfg.Camera(cnt).InsertBridgeSink(inBridge[cnt]);
                bridgeSrcFilter[cnt] = (IBaseFilter)inBridge[cnt].Bridge.InsertSourceFilter(sink,Graph);
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="MuxGraph"/> class.
        /// </summary>
        ~MuxGraph()
        {
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
        }
    }
}
//
// EOF: MuxGraph.cs
