/**
 * File: VgvBridge.cs
 * 
 *	Copyright © 2016-2017 by City Council Video.  All rights reserved.
 *
 *	$Id: /Video_Test_Fixture/VgvBridge.cs,v $
 */
/**
*	Encapsulates the GMFBridge object for the VGV applications
*
*	Author:			Fred Koschara
*	Creation Date:	April seventh, 2017
*	Last Modified:	April 26, 2017 @ 11:42 am
*
*	Revision History:
*	   Date		  by		Description
*	2017/04/26	wfredk	original development
*		|						|
*	2017/04/07	wfredk	original development
*/
using System;
using DirectShowLib;
using GMFBridgeLib;

using Utility.VgvUtility;

namespace Video_Test_Fixture
{
    /// <summary>
    /// encapsulates the GMFBridge object for the VGV applications
    /// </summary>
    public class VgvBridge
    {
//      private GlobalConfig cfg = null;
//      private IBaseFilter filterIn = null;
//      private IBaseFilter filterOut = null;
        private IBaseFilter filterSink = null;
        private IBaseFilter filterSrc = null;
        private IGMFBridgeController bridge = null;
        // the sink graph, feeding out from the bridge with a source filter
        private IGraphBuilder graphSink = null;
        // the source graph, feeding into the bridge with a sink filter
        private IGraphBuilder graphSrc = null;

        /// <summary>
        /// Gets the bridge.
        /// </summary>
        /// <value>
        /// The bridge.
        /// </value>
        public IGMFBridgeController Bridge
        {   get
            {   return bridge;
            }
        }

        // --------------------------------------------------------------------

        // helper method for eliminating code duplication
        private void CreateBridge()
        {
            bridge = (IGMFBridgeController)new GMFBridgeController();
            bridge.AddStream(1,eFormatType.eUncompressed,1);
        }

        /// <summary>
        /// constructor - initializes a new anonymous instance of
        /// the <see cref="VgvBridge"/> class
        /// </summary>
        public VgvBridge()
        {
            CreateBridge();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VgvBridge"/> class
        /// with its output (sink filter) connected to the source graph
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="feedPin">The feed pin.</param>
        public VgvBridge(IGraphBuilder graph,IPin feedPin)
        {
            int hr;

            CreateBridge();

            graphSrc = graph;

            filterSink = (IBaseFilter)bridge.InsertSinkFilter(graph);

//          VgvUtil.ListPins(filterSink);

            // connect input pin and sink filter
            hr = graph.ConnectDirect(feedPin,VgvUtil.GetPin(filterSink,"Input 1"),null);
            VgvUtil.checkHR(hr,"Can't connect feed pin and output filter");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VgvBridge"/> class.
        /// </summary>
        /// <param name="destPin">The dest pin.</param>
        /// <param name="graph">The graph.</param>
        /// <exception cref="NotImplementedException"></exception>
        public VgvBridge(IPin destPin,IGraphBuilder graph)
        {
//          int hr;

            CreateBridge();

            graphSink = graph;

            // TODO !!
            throw new NotImplementedException();
        }

        // --------------------------------------------------------------------
        /// <summary>
        /// destructor
        /// </summary>
        ~VgvBridge()
        {
            // TODO
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// Connects or disconnects the graphs.
        /// 
        /// N.B. The graphs should not be connected until the downstream graph
        ///     has been fully constructed.
        /// N.B. The grsphs must be disconnected before either is stopped.
        /// </summary>
        /// <param name="bConnect">if set to <c>true</c> [b connect].</param>
        public void ConnectGraphs(bool bConnect=true)
        {
            if (bConnect) // connect the two graphs
                bridge.BridgeGraphs(filterSink,filterSrc);
            else bridge.BridgeGraphs(null,null);    // disconnect
        }

        /// <summary>
        /// connects the source graph as the input to the bridge
        ///
        /// inserts a sink filter into the graph and connects the graph output pin
        /// to the bridge's input (the input pin of the sink filter)
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="feedPin">The feed pin.</param>
        /// <returns></returns>
        public IBaseFilter ConnectInput(IGraphBuilder graph,IPin feedPin)
        {
            int hr;

            graphSink = graph;

            filterSink = (IBaseFilter)bridge.InsertSinkFilter(graph);

            // connect feed pin and sink filter
            hr = graph.ConnectDirect(feedPin,VgvUtil.GetPin(filterSink,"Input 1"),null);
            VgvUtil.checkHR(hr,"Can't connect feed pin and output filter");

            return filterSink;
        }

        /// <summary>
        /// Connects the output of the bridge as the input for the downstream graph
        /// </summary>
        /// <param name="destPin">The dest pin.</param>
        /// <param name="graph">The graph.</param>
        /// <returns></returns>
        public IBaseFilter ConnectOutput(IPin destPin,IGraphBuilder graph)
        {
            int hr;

            graphSink = graph;

            if (filterSink == null)
                return null;

            filterSrc = (IBaseFilter)bridge.InsertSourceFilter(filterSink,graph);

//          VgvUtil.ListPins(filterSrc);

            // connect source filter and destination pin
            hr = graph.ConnectDirect(VgvUtil.GetPin(filterSrc,"Output 1"),destPin,null);
            VgvUtil.checkHR(hr,"Can't connect input filter and destination pin");

            return filterSrc;
        }
    }
}
//
// EOF: VgvBridge.cs
