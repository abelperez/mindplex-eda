
using System;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using System.Messaging;
using Threading = System.Threading;
using Collections = System.Collections;
using Regex = System.Text.RegularExpressions;
using Cryptography = System.Security.Cryptography;
using DirectoryServices = System.DirectoryServices;
using Compilers = _1800Communications.Utils.Compilers;
using System.Globalization;
using Microsoft.Win32;
using HexValidEmailLib;
using Base32Encoding;

namespace Mindplex.Core.Event.Rule
{
    class RulesEngine
    {
    }
}


//namespace _1800Communications.AggregationSystem {
//    namespace _CoreLibrary {

//        public enum SyslogTrigger {
//            Send,
//            Receive,
//            Both
//        }

//        /// <remarks>
//        /// The entire Aggregation system stems off of the MessageQueueLibrary.MessageQueueListener 
//        /// class.  This class handles a number of basic tasks such as creating and connecting to 
//        /// queues for input and output, queue pooling to minimize connection time, and a number of 
//        /// functions used by these routines.
//        /// 
//        /// The class is expected to be inherited and various pieces of functionality overridden as 
//        /// necessary.
//        /// Version 1.1.1: added support for the Monitor flag. monitor messages are off by default.
//        /// </remarks>
//        //[Version("1.1.1")]
//        public abstract class MessageQueueListener {
//            #region Protected Class Properties
			
//            /// <summary>
//            /// Lock object.
//            /// </summary>
//            protected static object m_oLockObject = new object();
//            /// <summary>
//            /// Whether to write Receive and Send messages to the Monitor queue.
//            /// </summary>
//            protected bool m_bMonitor = false;
//            /// <summary>
//            /// Routine to call back when operation of the thread is complete.
//            /// This allows us to keep a thread counter so we know when all
//            /// threads are dead.
//            /// </summary>
//            protected MessageQueueSpawner.ThreadCompleteCallback _ThreadComplete = null;

//            /// <summary>
//            /// This object is used for thread locking when sending to the monitor queue
//            /// </summary>
//            protected        object                  _MonitorQueueMonitor  = new object();
//            /// <summary>
//            /// The node all configuration of this element is stored in.  Typically this
//            /// would be a direct child of the DocumentElement, though that is not necessary.
//            /// </summary>
//            protected        XmlElement          _ConfigNode           = null;
//            /// <summary>
//            /// The queue to report all errors to (opened when needed the first time)
//            /// </summary>
//            protected        MessageQueue  _ErrorQueue           = null;
//            /// <summary>
//            /// The queue to send debugging messages to (opened when needed the first time)
//            /// </summary>
//            protected        MessageQueue  _DebugQueue           = null;
//            /// <summary>
//            /// The queue to send monitor messages to (opened when needed the first time)
//            /// </summary>
//            protected        MessageQueue  _MonitorQueue         = null;
//            /// <summary>
//            /// The queue to send logging messages to (opened when needed the first time)
//            /// </summary>
//            protected        MessageQueue  _LogQueue             = null;
//            /// <summary>
//            /// This is a collection of Queues to check on each pass for new messages.
//            /// Populated by the config file.
//            /// </summary>
//            protected        Collections.ArrayList   _InputQueues          = new Collections.ArrayList();
//            /// <summary>
//            /// These are all destinations queues used since the tool was started.
//            /// These may be dynamically generated, so they are not populated until used
//            /// for the first time.  They are maintained to reduce connection time on 
//            /// subsequent sends
//            /// </summary>
//            protected        Collections.ArrayList   _OutputQueues         = new Collections.ArrayList();
//            /// <summary>
//            /// The name of the queue used for debugging.  If this value is blank, no debug
//            /// messages are sent.  This can be overridden in each config file.
//            /// </summary>
//            protected        string                  _DebugQueueName       = "";
//            /// <summary>
//            /// The name of the queue to send error messages to.  This is by default
//            /// "Errors" but this can be overridden in each config file and tool's config 
//            /// nodes.
//            /// </summary>
//            protected        string                  _ErrorQueueName       = ".\\PRIVATE$\\Aggregator.Error";
//            /// <summary>
//            /// The monitor queue recieves a message when any other queue sends or receives
//            /// a message.  Again, this can be overridden in each config file.
//            /// </summary>
//            protected        string                  _MonitorQueueName     = ".\\Monitor";
//            /// <summary>
//            /// The Log receives a message whenever a component performs an action.
//            /// Overridable in both config file and tool config nodes.
//            /// </summary>
//            protected        string                  _LogQueueName         = "Logs";
//            /// <summary>
//            /// This is what's reported any time the tool needs to identify itself
//            /// (for logging, monitoring, etc.)
//            /// </summary>
//            protected        string                  _ToolName             = "Tool Name Not Set";
//            /// <summary>
//            /// Every queue, no matter what, gets the basequeue prepended to it.  The 
//            /// default value for this is ".\Aggregator".  However, this can be altered
//            /// by config files on a file by file or node by node basis.
//            /// </summary>
//            protected        string                  _BaseQueue            = ".\\Aggregator";
//            //	This was used to store the XSL to strip namespaces out, but I believe this is obsolete
//            //	protected        string                  _XSLStripNamespaces   = null;		// Loaded from Resource File
//            /// <summary>
//            /// Set to true to stop the thread, otherwise false;
//            /// </summary>
//            protected        bool                    _StopOperation        = true;
//            /// <summary>
//            /// Set to true while the thread is running, false once it's shut down
//            /// </summary>
//            protected        bool                    _Processing           = false;
//            /// <summary>
//            /// Set to true to pause operation.  This just prevents message queues from being read
//            /// </summary>
//            protected        bool                    _Paused               = false;
//            /// <summary>
//            /// This is set by the ErrorHandler so that it doesn't overwrite any LastQueue values
//            /// </summary>
//            protected        bool                    _Silent               = false;
//            /// <summary>
//            /// The thread running, doing the work.  This can be used for some tricky thread operation
//            /// stuff if absolutely needed by the spawner
//            /// </summary>
//            protected        System.Threading.Thread _OperatingThread      = null;
//            /// <summary>
//            /// Number of messages read by this thread
//            /// </summary>
//            protected        int                     _Incoming             = 0;
//            /// <summary>
//            /// Number of messages written by this thread
//            /// </summary>
//            protected        int                     _Outgoing             = 0;
//            /// <summary>
//            /// Range of times when this component can be active
//            /// </summary>
//            protected        DateTimeRanges          _ActiveTimes          = null;
//            /// <summary>
//            /// Range of times when this component can not be active
//            /// </summary>
//            protected        DateTimeRanges          _InactiveTimes        = null;
//            /// <summary>
//            /// This is used to offset all date times used by the system (except those for
//            /// display purposes).  The value comes passed in to the Listener constructor.
//            /// The purpose of this was to offset active and inactive times.  When three
//            /// machines' databasers fire up at the exact same time, the database starts
//            /// throw deadlocks.  This allows some breathing room between the machines.
//            /// </summary>
//            protected        System.TimeSpan         _TimeOffset           = new TimeSpan(0, 0, 0, 0, 0);
//            /// <summary>
//            /// The incoming queue path.
//            /// </summary>
//            protected string m_sIncomingQueuePath;
//            /// <summary>
//            /// The default cancel poll interval.
//            /// </summary>
//            protected const int DefaultCancelPollInterval = 500;
//            /// <summary>
//            /// This is how often the tool checks to see if a cancel command has been called.
//            /// </summary>
//            protected int m_iCancelPollInterval = DefaultCancelPollInterval;
//            /// <summary>
//            /// Whether the tool should enforce a minimum time between processed messages.
//            /// </summary>
//            protected bool m_bThrottleInputMessages = false;
//            /// <summary>
//            /// SOAP Namespace URN
//            /// </summary>
//            protected const  string                  _SoapURN              = "http://schemas.xmlsoap.org/soap/envelope/";
//            /// <summary>
//            /// This is the registry entry for the aggregator configuration (used to get
//            /// information needed to set permissions).
//            /// </summary>
//            protected const  string                  _QueueUserGroupsKey   = "SOFTWARE\\1-800 Communications\\Aggregator";
//            /// <summary>
//            /// Encryption key used when none is provided to an Encrypt/Decrypt routine
//            /// </summary>
//            private   const  string                  _DefaultEncryptionKey = "a8gR3Ge+";
//            /// <summary>
//            /// Random number generator.
//            /// </summary>
//            protected Random m_rndRandomNumberGenerator = new Random();
//            /// <summary>
//            /// A flag that says whether to send Log messages.
//            /// </summary>
//            protected bool m_bSendLogMessages = false;
//            /// <summary>
//            /// A flag that says whether the tool is active.
//            /// </summary>
//            protected bool m_bActive = true;

//            /// <summary>
//            /// Various performance counters
//            /// </summary>
//            //			protected        System.Diagnostics.PerformanceCounter _PerformanceRead              = null;
//            //			protected        System.Diagnostics.PerformanceCounter _PerformanceReadAll           = null;
//            //			protected        System.Diagnostics.PerformanceCounter _PerformanceWrite             = null;
//            //			protected        System.Diagnostics.PerformanceCounter _PerformanceWriteAll          = null;
//            //			protected        System.Diagnostics.PerformanceCounter _PerformanceProcessing        = null;
//            //			protected        System.Diagnostics.PerformanceCounter _PerformanceProcessingBase    = null;
//            //			protected        System.Diagnostics.PerformanceCounter _PerformanceProcessingAll     = null;
//            //			protected        System.Diagnostics.PerformanceCounter _PerformanceProcessingAllBase = null;

//            #endregion

//            #region Accessors
//            /// <summary>
//            /// Returns the name of this component
//            /// </summary>
//            public string Component {
//                get {
//                    return this._ToolName;
//                }
//            }

//            /// <summary>
//            /// Returns the config file this tool is using
//            /// </summary>
//            public XmlNode Config {
//                get {
//                    return this._ConfigNode;
//                }
//            }

//            /// <summary>
//            /// Number of messages read
//            /// </summary>
//            public int Incoming {
//                get {
//                    return _Incoming;
//                }
//            }
			
//            /// <summary>
//            /// Number of messages written
//            /// </summary>
//            public int Outgoing {
//                get {
//                    return _Outgoing;
//                }
//            }


//            /// <summary>
//            /// Paused status
//            /// </summary>
//            public bool IsPaused {
//                get {
//                    return _Paused;
//                }
//            }


//            /// <summary>
//            /// Pauses operation
//            /// </summary>
//            public virtual void Pause() {
//                _Paused = true;
//            }


//            /// <summary>
//            /// Resumes paused operation
//            /// </summary>
//            public virtual void Resume() {
//                _Paused = false;
//            }


//            /// <summary>
//            /// Internally used to create a new GUID
//            /// </summary>
//            private string NewGuid {
//                get {
//                    return System.Guid.NewGuid().ToString("D");
//                }
//            }

//            /// <summary>
//            /// Set a flag that says whether the tool is active (based on the time of day).
//            /// Override this method to perform additional tasks (like locking or unlocking files)
//            /// when the tool is activated or deactivated.
//            /// </summary>
//            protected virtual bool Active
//            {
//                get
//                {
//                    return m_bActive;
//                }
//                set
//                {
//                    if (value != m_bActive)
//                    {
//                        XmlNodeList xnlNotifications = null;
//                        if (value)
//                        {
//                            //"activated" notifications
//                            xnlNotifications = _ConfigNode.SelectNodes("activity/notify[@on='activate' or @on='change']");
//                        }
//                        else
//                        {
//                            //"deactivated" notifications
//                            xnlNotifications = _ConfigNode.SelectNodes("activity/notify[@on='deactivate' or @on='change']");
//                        }
//                        //create an XmlNamespaceManager
//                        XmlNamespaceManager xnmNamespaceManager = new XmlNamespaceManager(new NameTable());
//                        xnmNamespaceManager.AddNamespace(String.Empty, "urn:none");
//                        xnmNamespaceManager.AddNamespace("soap", _SoapURN);
//                        foreach (XmlElement xeNotify in xnlNotifications)
//                        {
//                            string sDatasetId = "";
//                            if (xeNotify.HasAttribute("datasetid"))
//                            {
//                                sDatasetId = xeNotify.GetAttribute("datasetid");
//                            }
//                            //get the notification message
//                            string sData = GetParameter("data", xeNotify, null, null);
//                            //create the notification application
//                            StringWriter swWriter = new StringWriter();
//                            XmlTextWriter xtwWriter = new XmlTextWriter(swWriter);
//                            xtwWriter.WriteStartElement("soap", "Envelope", _SoapURN);
//                            xtwWriter.WriteAttributeString("soap", "id", _SoapURN, Guid.NewGuid().ToString("D"));
//                            xtwWriter.WriteStartElement("soap", "Header", _SoapURN);
//                            xtwWriter.WriteStartElement("action");
//                            xtwWriter.WriteAttributeString("index", "1");
//                            xtwWriter.WriteEndElement();
//                            xtwWriter.WriteEndElement();
//                            xtwWriter.WriteStartElement("soap", "Body", _SoapURN);
//                            xtwWriter.WriteStartElement("dataset");
//                            xtwWriter.WriteAttributeString("id", sDatasetId);
//                            xtwWriter.WriteAttributeString("actor", "1");
//                            xtwWriter.WriteAttributeString("index", "1");
//                            xtwWriter.WriteAttributeString("tool", _ToolName);
//                            xtwWriter.WriteRaw(sData);
//                            xtwWriter.WriteEndElement();
//                            xtwWriter.WriteEndElement();
//                            xtwWriter.WriteEndElement();
//                            //create the XmlDocument
//                            XmlDocument xdNotification = new XmlDocument();
//                            System.Diagnostics.Debug.WriteLine(swWriter.ToString());
//                            xdNotification.LoadXml(swWriter.ToString());
//                            foreach (XmlElement xeDestination in xeNotify.SelectNodes("destinations"))
//                            {
//                                SendApplication(xdNotification, xeDestination, "", "", xnmNamespaceManager);
//                            }
//                        }
//                        m_bActive = value;
//                    }
//                }
//            }

//            #endregion
			
//            /// <summary>
//            /// Default Initialization Block.  Tools may override this routine if they have custom 
//            /// code which needs to be run or extra pieces to pull out of the configuration.  If the
//            /// routine is not overridden, it relies on Attributes of the Listener class to specify
//            /// critical pieces of data and generates a generic configuration using what it finds.
//            /// </summary>
//            /// <param name="UseThreading">Obsolete, used to determine whether or not to use events</param>
//            /// <param name="ConfigNode">This configuration Element</param>
//            /// <param name="ThreadComplete">The delegate to call when operation has been completed</param>
//            /// <param name="Offset">This machine's overall time offset</param>
//            /// <param name="bMonitor">Whether to write receive and send messages to the Monitor queue.</param>
//            //public virtual void Initialize(bool UseThreading, XmlElement ConfigNode, MessageQueueSpawner.ThreadCompleteCallback ThreadComplete, TimeSpan Offset, bool bMonitor)
//            //{
//            //    // Since we haven't been overridden....
//            //    // Get this tool name
//            //    _ToolName = null;
//            //    foreach (ListenerAttribute Att in this.GetType().GetCustomAttributes(typeof(ListenerAttribute), true))
//            //    {
//            //        if (Att.ToolName != null)
//            //        {
//            //            _ToolName = Att.ToolName;
//            //        }
//            //    }
//            //    // No tool name means bad component author.  Throw an Exception
//            //    if (_ToolName == null)
//            //    {
//            //        throw new Exception(this.GetType().ToString() + " class does not have a Listener.ToolName attribute or implement a constructor");
//            //    }
//            //    // Set other basic config junk
//            //    _TimeOffset = Offset;
//            //    _ConfigNode = ConfigNode;
//            //    _ThreadComplete = ThreadComplete;
//            //    m_bMonitor = bMonitor;
//            //    if (ConfigNode != null && ConfigNode.HasAttribute("log"))
//            //    {
//            //        string sLog = ConfigNode.GetAttribute("log").Trim().ToLower();
//            //        m_bSendLogMessages = (sLog == "yes" || sLog == "true");
//            //    }
//            //}
						
//            /// <summary>
//            /// Sets the basic queues (Monitor, Error, Debug, Log and Base)
//            /// based on the data passed in.
//            /// </summary>
//            /// <param name="InfoElement">
//            ///		XmlElement with attributes specifying the queue names
//            ///	</param>
//            public virtual void SetBasicQueues(XmlElement InfoElement) {
//                // Get All Queue Names
//                string MonitorQueueName = InfoElement.GetAttribute("monitorqueue");
//                string ErrorQueueName   = InfoElement.GetAttribute("errorqueue");
//                string DebugQueueName   = InfoElement.GetAttribute("debugqueue");
//                string LogQueueName     = InfoElement.GetAttribute("logqueue");
//                string BaseQueue        = InfoElement.GetAttribute("basequeue");
				
//                // If We're not midstream, update all names to include BaseQueue 
//                // (This "if" should probably be nuked)
//                if (!_Processing) {
//                    if (BaseQueue != "")
//                        _BaseQueue = BaseQueue;

//                    if ((!_BaseQueue.EndsWith(".")) && (!_BaseQueue.EndsWith("\\")))
//                        _BaseQueue += ".";

//                    if (DebugQueueName != "")
//                        _DebugQueueName = _BaseQueue + DebugQueueName;
//                    if (ErrorQueueName != "")
//                        _ErrorQueueName = _BaseQueue + ErrorQueueName;
//                    if (LogQueueName != "")
//                        _LogQueueName   = _BaseQueue + LogQueueName;
//                    if (MonitorQueueName != "")
//                        _MonitorQueueName   = MonitorQueueName;

//                    // Close any queues currently known about
//                    if (_ErrorQueue != null) {
//                        _ErrorQueue.Close();
//                        _ErrorQueue = null;
//                    }

//                    if (_LogQueue != null) {
//                        _LogQueue.Close();
//                        _LogQueue = null;
//                    }

//                    if (_DebugQueue != null) {
//                        _DebugQueue.Close();
//                        _DebugQueue = null;
//                    }

//                    if (_MonitorQueue != null) {
//                        _MonitorQueue.Close();
//                        _MonitorQueue = null;
//                    }
//                }
//            }

			
//            /// <summary>
//            /// This lets the author add extra functionality whenever this tool starts up
//            /// </summary>
//            protected virtual void StartListening() {
//            }
			
//            /// <remarks>
//            /// DoWork is the routine called which begins listening for applications.  This
//            /// is only called once concurrently for a given object.
//            /// 
//            /// The routine initializes queues to listen to by connecting and setting up an event
//            /// handler for new messages.  Then it loops, polling for new messages on each pass.
//            /// Each loop it sleeps for _CancelPollInterval milliseconds.
//            /// 
//            /// Looping continues until _StopOperation is set to false by Cancel().  At that point
//            /// the loop finishes and the routine ends.
//            /// 
//            /// This routine is designed to be run as a thread, which is created in Run().
//            /// 
//            /// This routine may be overridden as necessary, though it is not recommended.
//            /// </remarks>
//            protected virtual void DoWork() {
//                // Sleep for a second so other components can start up
//                Threading.Thread.Sleep(1000);
//                try {
//                    //messages per second
//                    if (_ConfigNode.HasAttribute("inputmessagespersecond")) {
//                        //if the inputmessagespersecond attribute is specified, we will throttle input messages regardless of whether the value is valid
//                        m_bThrottleInputMessages = true;
//                        try {
//                            //get the maximum number of messages to process per second
//                            double dInputMessagesPerSecond = Convert.ToDouble(_ConfigNode.GetAttribute("inputmessagespersecond"));
//                            if (dInputMessagesPerSecond > 0.0 && dInputMessagesPerSecond <= 1000.0) {
//                                //calculate the number of milliseconds to wait between messages
//                                m_iCancelPollInterval = (int)(1000 / dInputMessagesPerSecond);
//                            }
//                        }
//                        catch {
//                            //ignore bad values
//                        }
//                    }
//                    // Allow Custom Code
//                    StartListening();
//                    // Set up periods of (in)activity
//                    _ActiveTimes   = new DateTimeRanges();
//                    _InactiveTimes = new DateTimeRanges();
//                    XmlNodeList ActiveTimeNodes = _ConfigNode.SelectNodes("activity/active");
//                    if (ActiveTimeNodes.Count > 0) {
//                        _ActiveTimes.Load(ActiveTimeNodes, _TimeOffset);
//                    }
//                    else {
//                        _ActiveTimes = null;
//                    }
//                    XmlNodeList InactiveTimeNodes = _ConfigNode.SelectNodes("activity/inactive");
//                    if (InactiveTimeNodes.Count > 0) {
//                        _InactiveTimes.Load(InactiveTimeNodes, _TimeOffset);
//                    }
//                    else {
//                        _InactiveTimes = null;
//                    }
//                    XmlNodeList SourceNodeSet = _ConfigNode.SelectNodes("sources/queue");
//                    foreach (XmlElement SourceNode in SourceNodeSet) {
//                        string sQueuePath = SourceNode.InnerText.Trim();
//                        if (SourceNode.GetAttribute("type") != "absolute") {
//                            sQueuePath = _BaseQueue + sQueuePath;
//                            if (!sQueuePath.ToLower().StartsWith("formatname") && sQueuePath.ToLower().IndexOf("private$") > -1) {
//                                sQueuePath = "FormatName:DIRECT=OS:" + sQueuePath;
//                            }
//                        }
//                        MessageQueue NewQueue = OpenQueue(sQueuePath);
//                        _InputQueues.Add(NewQueue);
//                    }
//                    // Get ready for messages which are spawned by a timed process
//                    XmlNodeList TimerNodeSet = _ConfigNode.SelectNodes("sources/timer");
//                    Timer[] TimeBetweenRequests = new Timer[TimerNodeSet.Count];
//                    int counter = 0;
//                    foreach (XmlElement TimerElement in TimerNodeSet) {
//                        if (TimerElement != null) {
//                            int days = int.Parse("0" + TimerElement.GetAttribute("days"));
//                            int hours = int.Parse("0" + TimerElement.GetAttribute("hours"));
//                            int minutes = int.Parse("0" + TimerElement.GetAttribute("minutes"));
//                            int seconds = int.Parse("0" + TimerElement.GetAttribute("seconds")); 
//                            TimeBetweenRequests[counter] = new Timer();
//                            TimeBetweenRequests[counter].Interval = (((days * 24 + hours) * 60 + minutes) * 60 + seconds) * 1000;
//                            if (TimerElement.SelectSingleNode("*") == null) {
//                                XmlElement SoapMessage = TimerElement.OwnerDocument.CreateElement("soap:Envelope", _SoapURN);
//                                SoapMessage.SetAttribute("id", _SoapURN, Guid.NewGuid().ToString("D"));
//                                SoapMessage.AppendChild(TimerElement.OwnerDocument.CreateElement("soap:Body"));
//                                TimeBetweenRequests[counter].MessageParent = SoapMessage;
//                            }
//                            else {
//                                TimeBetweenRequests[counter].MessageParent = (XmlElement)TimerElement.SelectSingleNode("*");
//                            }
//                        }
//                        if (TimeBetweenRequests[counter].Interval == 0) {
//                            TimeBetweenRequests[counter].Interval = 5 * 60 * 1000; // 5 minute default
//                        }
//                        counter++;
//                    }
//                    // Until the thread is canceled using Cancel()....
//                    while (!_StopOperation) {
//                        int iStartTime = 0;
//                        int iFinishTime = 0;
//                        bool FoundMessage = false;
//                        bool bActive = true;
//                        if (_ActiveTimes   != null) {
//                            bActive = bActive && _ActiveTimes.Current;
//                        }
//                        if (_InactiveTimes != null) {
//                            bActive = bActive && !_InactiveTimes.Current;
//                        }
//                        //override Active to take action when the tool changes active states.
//                        Active = bActive;
//                        // If we're allowed to process....
//                        if (Active && !_Paused) {
//                            Collections.ArrayList CurrentList = Timer.getActions(TimeBetweenRequests);
//                            // Create Any Timed Message Which are Ready
//                            foreach (Timer Current in CurrentList) {
//                                MessageRecieved(Current.MessageParent.OuterXml, string.Format("    {0,51}", "[Timed Message]"), null);
//                            }
//                            if (!_StopOperation) {
//                                // Loop over all inputs
//                                foreach (MessageQueue InputToCheck in _InputQueues) {
//                                    Message NewMessage = null;
//                                    try {
//                                        // Check for new message on queue
//                                        NewMessage = InputToCheck.Receive(TimeSpan.Zero);
//                                        try {
//                                            // Calculate metrics and send message off for processing
//                                            iStartTime = System.Environment.TickCount;
//                                            MessageRecieved(NewMessage, InputToCheck);
//                                            iFinishTime = System.Environment.TickCount;
//                                            // Record metrics
//                                            //_PerformanceProcessing.IncrementBy(FinishTime - StartTime);
//                                            //_PerformanceProcessingAll.IncrementBy(FinishTime - StartTime);
//                                            //_PerformanceProcessingBase.Increment();
//                                            //_PerformanceProcessingAllBase.Increment();
//                                            FoundMessage = true;
//                                        }
//                                        catch (Exception exx) {
//                                            // Something screwed up
//                                            ReportErrorNow(null, exx);
//                                        }
//                                    }
//                                    catch (System.Messaging.MessageQueueException mqe) {
//                                        //if timeout expired, ignore
//                                        if (mqe.MessageQueueErrorCode != MessageQueueErrorCode.IOTimeout) {
//                                            ReportErrorNow(null, mqe);
//                                        }
//                                    }
//                                    catch (Exception e) {
//                                        ReportErrorNow(null, e);
//                                    }
//                                    finally {
//                                        if (NewMessage != null) {
//                                            NewMessage.Dispose();
//                                        }
//                                    }
//                                }
//                            }
//                        }
//                        // If nothing was found, sleep.  You want to skip a sleep cycle is something
//                        // was found because there may be more.
//                        if (!FoundMessage) {
//                            Threading.Thread.Sleep(m_iCancelPollInterval);
//                        }
//                        else if (m_bThrottleInputMessages) {
//                            //also sleep if the "inputmessagespersecond" attribute is set
//                            int iMilliseconds = m_iCancelPollInterval - (iFinishTime - iStartTime);
//                            if (iMilliseconds > 0) {
//                                Threading.Thread.Sleep(iMilliseconds);
//                            }
//                        }
//                    }
//                }
//                catch (Exception exx) {
//                    // Another screwup somewhere
//                    ReportErrorNow(null, exx);
//                }
//                finally {
//                    // Close any queues being read
//                    foreach (MessageQueue InputToClose in _InputQueues) {
//                        if (InputToClose != null) {
//                            InputToClose.Close();
//                        }
//                    }
//                    // Report thread complete
//                    System.Console.WriteLine(DateTime.Now.ToString() + "            - " + "Async thread: Finished.");
//                    _ThreadComplete();
//                    // Allow custom shutdown code
//                    StopListening();
//                }
//                // We're done
//                _Processing = false;
//            }

//            /// <summary>
//            /// This lets the author add extra functionality whenever this tool shuts down
//            /// </summary>
//            protected virtual void StopListening() {
//            }


//            /// <summary>
//            /// This is the routine which is to be used to begin processing.  It returns a boolean
//            /// determining whether the processing was actually begun or not (primarily determined 
//            /// by whether the processing is currently running).
//            /// 
//            /// If the process is not currently running, a thread is created.  In all cases,  Run() 
//            /// should return immediately.
//            /// 
//            /// This method can not be overridden.
//            /// </summary>

//            public bool Run() {
//                // Set up performance stuff
//                //				SetupCategory();
//                //				CreateCounters();

//                if (!_Processing) {
//                    // Record that thread is running and actually run it.
//                    _Processing = true;
//                    _StopOperation = false;

//                    _OperatingThread = new Threading.Thread(new Threading.ThreadStart(this.DoWork));
//                    _OperatingThread.Name = _ToolName;
//                    _OperatingThread.Start();

//                    return true;
//                }
//                return false;
//            }


//            /// <summary>
//            /// Once the class is processing, Cancel() may be called at any time to allow the thread
//            /// to gracefully shut itself down.  Cancel returns immediately.  The calling application 
//            /// is notified of thread completion via a callback routine - ThreadComplete() which 
//            /// should be set at configuration time.
//            /// </summary>

//            public virtual void Cancel() {
//                if (_Processing) {
//                    System.Console.WriteLine(DateTime.Now.ToString() + "            - " + "Async thread: Cancelled.");
//                    _StopOperation = true;
//                }
//            }

//            /// <summary>
//            /// Typically, the MessageQueueListener will connect to a message queue and recieve SOAP
//            /// formatted messages.  As these messages come in, they need to be processed.  This
//            /// is the event handler which recieves these messages.  The message is read, sent to
//            /// ProcessApplication() for processing.  It is then stamped, the actions are sent to the
//            /// log queue, errors are sent to the error queue and the SOAP message is sent along to
//            /// the appropriate output queues.
//            /// 
//            /// This routine may be overridden as necessary, though it is not recommended.
//            /// </summary>
//            protected virtual void HandleNewMessage(object sender, ReceiveCompletedEventArgs e) {
//                MessageQueue IncomingQueue = (MessageQueue)sender;
//                Message Application = IncomingQueue.EndReceive(e.AsyncResult);
//                try {
//                    MessageRecieved(Application, IncomingQueue);
//                }
//                finally {
//                    if (Application != null) {
//                        Application.Dispose();
//                    }
//                }
//            }

//            /// <summary>
//            /// Takes a message which was recieved and begins the execution process.
//            /// Used to begin processing on a message queue message
//            /// </summary>
//            /// <param name="Application">Entire Message recieved</param>
//            /// <param name="IncomingQueue">Queue Message was recieved from</param>
//            protected virtual void MessageRecieved(Message Application, MessageQueue IncomingQueue) {
//                MessageRecieved(new System.IO.StreamReader(Application.BodyStream).ReadToEnd(), Application.Label, IncomingQueue);
//            }

//            /// <summary>
//            /// Takes a message which was recieved and begins the execution process.
//            /// Used to begin processing on an XML Element (Usually for timed messages)
//            /// </summary>
//            /// <param name="ApplicationParentNode"></param>
//            /// <param name="IncomingQueue"></param>
//            protected virtual void MessageRecieved(XmlElement ApplicationParentNode, MessageQueue IncomingQueue) {
//                string DatasetID   = System.Web.HttpUtility.HtmlAttributeEncode(ApplicationParentNode.GetAttribute("id"));
//                string MessageBody = "<soap:Envelope xmlns:soap='" + _SoapURN + "' soap:id='" + NewGuid + "'><soap:Header/><soap:Body><dataset id='" + DatasetID + "'>" + ApplicationParentNode.InnerXml + "</dataset></soap:Body></soap:Envelope>";

//                MessageRecieved(MessageBody, DatasetID, IncomingQueue);
//            }

//#region WORK HERE

//            /// <summary>
//            /// Takes a message which was recieved and begins the execution process.
//            /// Used to begin processing on a string whichcontains SOAP XML
//            /// </summary>
//            /// 
//            /// <param name="ApplicationXML"></param>
//            /// <param name="Label"></param>
//            /// <param name="IncomingQueue"></param>
//            /// 
//            protected virtual void MessageRecieved(string ApplicationXML, string Label, MessageQueue IncomingQueue) {
//                string IncomingQueuePath = m_sIncomingQueuePath = "";
//                if (IncomingQueue != null) {
//                    IncomingQueuePath = m_sIncomingQueuePath = IncomingQueue.Path;
//                }
//                // Set up a name table so we can query based on namespaces
//                NameTable           SoapNameTable        = new NameTable();
//                XmlNamespaceManager SoapNamespaceManager = new XmlNamespaceManager(SoapNameTable);
//                SoapNamespaceManager.AddNamespace(String.Empty, "urn:none");
//                SoapNamespaceManager.AddNamespace("soap", _SoapURN);
//                // This allows custom namespaces as defined in the config xml
//                foreach (XmlElement NamespaceNode in _ConfigNode.SelectNodes("namespaces/namespace")) {
//                    string NamespaceName = "";
//                    string NamespaceURN  = "urn:none";
//                    if (NamespaceNode.GetAttributeNode("name") != null) {
//                        NamespaceName = NamespaceNode.GetAttribute("name");
//                    }
//                    if (NamespaceNode.InnerText != "") {
//                        NamespaceURN = NamespaceNode.InnerText;
//                    }
//                    SoapNamespaceManager.AddNamespace(NamespaceName, NamespaceURN);
//                }
//                // Set up the Soap Message
//                XmlDocument  SoapMessage   = new XmlDocument(SoapNameTable);
//                //send a message to the monitor queue
//                MonitorRecieve(MessageType.Application, IncomingQueuePath, Label);
//                try {
//                    // Dump to screen, new message
//                    System.Console.WriteLine("{0} [{1,4}] ({2})", DateTime.Now, Label, _ToolName);
//                    // I need to find a better way to get a single attribute
//                    foreach (ListenerAttribute Att in this.GetType().GetCustomAttributes(typeof(ListenerAttribute), true)) {
//                        SoapMessage.LoadXml(ApplicationXML);
//                        //we now have the message. process any syslog nodes
//                        ProcessSyslogNodes(SyslogTrigger.Receive, SoapMessage, SoapNamespaceManager);
//                        // If this tool typically contributes to the message, the process it, otherwise just send it on
//                        if (Att.Contributes) {
//                            if (TryToProcessMessage(SoapMessage, SoapNamespaceManager, Label, IncomingQueue)) {
//                                SendApplication(SoapMessage, Label, IncomingQueuePath, SoapNamespaceManager);
//                            }
//                        }
//                        else {
//                            SendApplication(SoapMessage, Label, IncomingQueuePath, SoapNamespaceManager);
//                        }
//                    }
//                }
//                catch (MessageQueueException mexx) {
//                    // An error was caught deeper, we just need to send it
//                    mexx.Send(_ErrorQueueName);
//                    MonitorSend(MessageType.Error, "", _ErrorQueueName, null);
//                }
//                catch (Exception exx) {
//                    // A new error popped up, send it
//                    ReportErrorNow(null, exx);
//                }
//            }

			
//            /// <summary>
//            /// By this point, we have the individual action to process.  We
//            /// just need to process it and deal with what happens.
//            /// </summary>
//            /// <param name="SoapMessage"></param>
//            /// <param name="SoapNamespaceManager"></param>
//            /// <param name="Label"></param>
//            /// <param name="IncomingQueue"></param>
//            /// <param name="Action"></param>
//            /// <returns></returns>
//            public virtual bool ExecuteAction(XmlDocument SoapMessage, XmlNamespaceManager SoapNamespaceManager, string Label, MessageQueue IncomingQueue, XmlElement Action, XmlElement ActionParent) {
//                XmlElement NewChunk = null;
//                string UpdateMessage = "";
//                int RetryCount = 1;
//                MessageQueueException Error = null;
//                // Find out how many times to try this action in case of exception (default is 1)
//                if (ActionParent != null && ActionParent.GetAttributeNode("retrycount") != null) {
//                    try {
//                        RetryCount = int.Parse(ActionParent.GetAttribute("retrycount"));
//                    }
//                    catch {
//                    }
//                }
//                // Try to process the application
//                for (int I = 0; I < RetryCount; I++) {
//                    try {
//                        Error = null;
//                        NewChunk = ProcessApplication(SoapMessage, SoapNamespaceManager, Label, Action, ref UpdateMessage);
//                        break;
//                    }
//                    catch (MessageQueueException NewError) {
//                        Error = NewError;
//                    }
//                }
				
//                if (Error != null) {
//                    // If there were errors all [RetryCount] times, send an error message
//                    Error.Send(_ErrorQueueName);
//                    MonitorSend(MessageType.Error, "", _ErrorQueueName, null);
//                    return (ActionParent.GetAttribute("failure") == "continue");
//                }
//                else {
//                    // If it succeeded at all...
//                    string IncomingQueuePath = "";
//                    if (IncomingQueue != null) {
//                        IncomingQueuePath = IncomingQueue.Path;
//                    }					
//                    // if there was data back, stamp it in the app
//                    if (NewChunk != null) {
//                        string ActionID = null;
//                        bool bBlankDatasetID = false;
//                        if (ActionParent != null) {
//                            ActionID = ActionParent.GetAttribute("id");
//                            if (ActionParent.HasAttribute("allowblankdatasetid")) {
//                                try {
//                                    bBlankDatasetID = Convert.ToBoolean(ActionParent.GetAttribute("allowblankdatasetid"));
//                                }
//                                catch {
//                                    //ignore, default to false
//                                }
//                            }
//                        }
//                        StampApplication(SoapMessage, SoapNamespaceManager, ActionID, bBlankDatasetID, UpdateMessage, IncomingQueuePath, NewChunk, true, (ActionParent.GetAttribute("results").ToLower() != "ignore"));
//                    }
//                    // if there was a message back, log it
//                    if (UpdateMessage != "") {
//                        XmlElement ApplicationNode = (XmlElement)SoapMessage.SelectSingleNode("/soap:Envelope/soap:Body/dataset[@id='']/APPLICATION", SoapNamespaceManager);
//                        LogAction(ApplicationNode, IncomingQueuePath, UpdateMessage);
//                    }
//                    return true;
//                }
//            }


//            /// <summary>
//            /// This removes namespaces from an XML Element (Scott)
//            /// </summary>
//            /// <param name="xeElement"></param>
//            /// <returns></returns>
//            protected string RemoveNamespaces(XmlNode xeElement) {
//                StringWriter swOutput = new System.IO.StringWriter();
//                XmlTextWriter xtwWriter = new XmlTextWriter(swOutput);
//                //xtwWriter.WriteStartDocument();
//                XmlNodeReader xnrReader = new XmlNodeReader(xeElement);
//                while (xnrReader.Read()) {
//                    switch (xnrReader.NodeType) {
//                        case XmlNodeType.Element:
//                            xtwWriter.WriteStartElement(xnrReader.Name);
//                            if (xnrReader.HasAttributes) {
//                                while (xnrReader.MoveToNextAttribute()) {
//                                    if (xnrReader.Name != "xmlns") {
//                                        xtwWriter.WriteAttributeString(xnrReader.Name, xnrReader.Value);
//                                    }
//                                }
//                                xnrReader.MoveToElement();
//                            }
//                            if (xnrReader.IsEmptyElement) {
//                                xtwWriter.WriteEndElement();
//                            }
//                            break;
//                        case XmlNodeType.Text:
//                            xtwWriter.WriteString(xnrReader.Value);
//                            break;
//                        case XmlNodeType.CDATA:
//                            xtwWriter.WriteCData(xnrReader.Value);
//                            break;
//                        case XmlNodeType.ProcessingInstruction:
//                            xtwWriter.WriteProcessingInstruction(xnrReader.Name, xnrReader.Value);
//                            break;
//                        case XmlNodeType.Comment:
//                            xtwWriter.WriteComment(xnrReader.Value);
//                            break;
//                        case XmlNodeType.EntityReference:
//                            xtwWriter.WriteEntityRef(xnrReader.Name);
//                            break;
//                        case XmlNodeType.EndElement:
//                            xtwWriter.WriteEndElement();
//                            break;
//                    }
//                }
//                //xtwWriter.WriteEndDocument();
//                xtwWriter.Flush();
//                xtwWriter.Close();
//                xnrReader.Close();
//                string sOutput = swOutput.ToString();
//                return sOutput;
//            }

			
//            /// <summary>
//            /// Stamps the application with Header information and associates that with the data
//            /// being added to the application.
//            /// </summary>
//            /// <param name="SoapMessage">The Application</param>
//            /// <param name="SoapNamespaceManager">Namespace Manager for the Application</param>
//            /// <param name="ActionID">ID for this action</param>
//            /// <param name="bBlankDatasetID">A flag that says whether to allow a blank dataset ID</param>
//            /// <param name="UpdateMessage">Components Message about what was done</param>
//            /// <param name="IncomingQueuePath">Queue this message was read from</param>
//            /// <param name="NewChunk">The chunk to add to the application</param>
//            /// <param name="WriteHeader">Should the header be written</param>
//            /// <param name="WriteData">Should the data be written</param>
//            /// <returns></returns>
//            public XmlElement StampApplication(XmlDocument SoapMessage, XmlNamespaceManager SoapNamespaceManager, string ActionID, bool bBlankDatasetID, string UpdateMessage, string IncomingQueuePath, XmlNode NewChunk, bool WriteHeader, bool WriteData) {
//                // Don't bother if the app doesn't exist
//                if (SoapMessage != null) {
//                    if (ActionID == null) {
//                        ActionID = "";
//                    }
//                    if (ActionID == "" && !bBlankDatasetID) {
//                        ActionID = "[Unspecified]";
//                    }
//                    // Set an index so this ActionID can be uniquely identified
//                    int Index = SoapMessage.SelectNodes("/soap:Envelope/soap:Body/dataset[@id='" + ActionID + "']", SoapNamespaceManager).Count + 1;
//                    // Try to write the header data
//                    int ActorID = -1;
//                    if (WriteHeader) {
//                        ActorID = MarkUsersApplication(SoapMessage, SoapNamespaceManager, UpdateMessage, IncomingQueuePath);
//                    }
//                    // Try to write the NewChunk
//                    if (WriteData) {
//                        XmlElement NewDataSet = SoapMessage.CreateElement("dataset");
//                        NewDataSet.SetAttribute("id", ActionID);
//                        NewDataSet.SetAttribute("index", Index.ToString());
//                        NewDataSet.SetAttribute("tool", _ToolName);
//                        // If there was no actor, don't write the attribute
//                        if (ActorID > 0) {
//                            NewDataSet.SetAttribute("actor", ActorID.ToString());
//                        }
//                        // If there was actually a new chunk, slap it in there
//                        if (NewChunk != null) {
//                            try {
//                                XmlNode xnNamespacesStripped = NewDataSet.OwnerDocument.ReadNode(new XmlTextReader(RemoveNamespaces(NewChunk), XmlNodeType.Element, null));
//                                NewDataSet.AppendChild(xnNamespacesStripped);
//                            }
//                            catch (Exception exx) {
//                                ReportErrorNow(SoapMessage, exx);
//                                NewDataSet.AppendChild(NewDataSet.OwnerDocument.ImportNode(NewChunk, true));
//                            }
//                        }
//                        // Add to the Soap Message
//                        XmlNode Target = SoapMessage.SelectSingleNode("/soap:Envelope/soap:Body", SoapNamespaceManager);
//                        if (Target == null) {
//                            Target = SoapMessage.DocumentElement;
//                        }
//                        Target.AppendChild(NewDataSet);
//                        return NewDataSet;
//                    }
//                    else {
//                        return null;
//                    }
//                }
//                else {
//                    return null;
//                }
//            }

//            /// <summary>
//            /// This routine takes a message, finds the appropriate actions, executes
//            /// any necessary stylesheet, and executes each in turn.
//            /// </summary>
//            /// <param name="SoapMessage"></param>
//            /// <param name="SoapNamespaceManager"></param>
//            /// <param name="Label"></param>
//            /// <param name="IncomingQueue"></param>
//            /// <returns></returns>
//            private bool TryToProcessMessage(XmlDocument SoapMessage, XmlNamespaceManager SoapNamespaceManager, string Label, MessageQueue IncomingQueue) {
//                XmlNodeList ActionsToRun = _ConfigNode.SelectNodes("action");
//                bool OKToSend = true;
//                if (ActionsToRun.Count == 0) {
//                    OKToSend = ExecuteAction(SoapMessage, SoapNamespaceManager, Label, IncomingQueue, null, null);
//                }
//                else {
//                    foreach (XmlElement Action in ActionsToRun) {
//                        if (CheckTests(Action, SoapMessage, SoapNamespaceManager)) {		
//                            foreach (XmlElement ActionDescription in Action.SelectNodes("*")) {
//                                XmlElement ActionCommand = null;
//                                switch (ActionDescription.Name) {
//                                    case "test":
//                                        break;
//                                    case "xsl:stylesheet":
//                                        try {
//                                            ActionCommand = Transform(SoapMessage, ActionDescription);
//                                        }
//                                        catch (System.Exception exx) {
//                                            ReportErrorNow(SoapMessage, exx);
//                                            ActionCommand = null;
//                                        }
//                                        break;
//                                    default:
//                                        ActionCommand = ActionDescription;
//                                        break;
//                                }
//                                if (ActionCommand != null) {
//                                    if (ActionCommand.Name == "action") {
//                                        foreach (XmlElement TempAction in ActionCommand.SelectNodes("*")) {
//                                            if (CheckTests(TempAction, SoapMessage, SoapNamespaceManager)) {
//                                                OKToSend = OKToSend && ExecuteAction(SoapMessage, SoapNamespaceManager, Label, IncomingQueue, TempAction, Action);
//                                            }
//                                        }
//                                    }
//                                    else if (CheckTests(ActionCommand, SoapMessage, SoapNamespaceManager)) {
//                                        OKToSend = OKToSend && ExecuteAction(SoapMessage, SoapNamespaceManager, Label, IncomingQueue, ActionCommand, Action);
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }
//                return OKToSend;
//            }

			
//            /// <summary>
//            /// Marks the Header section of the user's application with the 
//            /// activity just performed
//            /// </summary>
//            /// <param name="SoapMessage">User's Application</param>
//            /// <param name="SoapNamespaceManager">Namespace Manager for User's Application</param>
//            /// <param name="UpdateMessage">The text returned by the update code</param>
//            /// <param name="Source">Queue this message was read from</param>
//            /// <returns></returns>
//            private int MarkUsersApplication(XmlDocument SoapMessage, XmlNamespaceManager SoapNamespaceManager, string UpdateMessage, string Source) {
//                // Find the header block
//                XmlElement Header = (XmlElement)SoapMessage.SelectSingleNode("/soap:Envelope/soap:Header", SoapNamespaceManager);
//                if (Header == null) {
//                    // If none exists and there is a soap:Envelope, create the header
//                    XmlElement Envelope = (XmlElement)SoapMessage.SelectSingleNode("/soap:Envelope", SoapNamespaceManager);
//                    if (Envelope == null)
//                        return -1;
//                    Header = SoapMessage.CreateElement("soap", "Header", _SoapURN);
//                    Envelope.AppendChild(Header);
//                }

//                // Create, populate and add various pieces of data
//                XmlElement ActionNode  = SoapMessage.CreateElement("action");
//                XmlElement ActorNode   = SoapMessage.CreateElement("actor");
//                XmlElement FromNode    = SoapMessage.CreateElement("from");
//                XmlElement TimeNode    = SoapMessage.CreateElement("timestamp");
//                XmlElement MachineNode = SoapMessage.CreateElement("machine");
//                XmlElement DescNode    = SoapMessage.CreateElement("description");

//                ActorNode.AppendChild(SoapMessage.CreateTextNode(_ToolName));
//                FromNode.AppendChild(SoapMessage.CreateTextNode(Source));
//                TimeNode.AppendChild(SoapMessage.CreateTextNode(DateTime.Now.ToString()));
//                MachineNode.AppendChild(SoapMessage.CreateTextNode(System.Net.Dns.GetHostName()));
//                DescNode.AppendChild(SoapMessage.CreateTextNode(UpdateMessage));
				
//                ActionNode.AppendChild(ActorNode);
//                ActionNode.AppendChild(FromNode);
//                ActionNode.AppendChild(TimeNode);
//                ActionNode.AppendChild(MachineNode);
//                ActionNode.AppendChild(DescNode);

//                // Try to set an index up
//                try {
//                    ActionNode.SetAttribute("index", (Header.SelectNodes("action").Count + 1).ToString());
//                }
//                catch {
//                    ActionNode.SetAttribute("index", "1");
//                }
				
//                // Add this header to the document
//                Header.AppendChild(ActionNode);

//                // Return the Index of this guy to be put in the new data chunk
//                return Header.SelectNodes("action").Count;
//            }

//            /// <summary>
//            /// Determine the actual queue path to use based on the queue node and incoming queue.
//            /// </summary>
//            /// <param name="sPath">The specified queue path.</param>
//            /// <param name="xeQueue">The queue element from the config file.</param>
//            /// <param name="sIncomingQueueName">The incoming queue name.</param>
//            /// <returns>
//            /// if the "type" attribute on the queue element is "full":
//            ///		returns the base queue name (which already has a dot on the end of it) followed by the specified queue name.
//            ///	if the "type" attribute on the queue element is "absolute":
//            ///		returns the specified queue path, exactly.
//            /// if the "type" attribute on the queue is something else, or not specified:
//            ///		returns the incoming queue name followed by a dot and the specified queue name.
//            ///	for non-absolute queues (full and anything else), if the "machine" attribute is specified:
//            ///		returns a queue name as specified above, with the machine part replaced with the value of the "machine" attribute.
//            ///	unless type="absolute":
//            ///		Private queues will be returned in the format
//            ///			FormatName:DIRECT=OS:{queue name}
//            ///		Public queues will be returned in the format
//            ///			{queue name}
//            /// </returns>
//            protected string ResolveQueuePath(string sPath, XmlElement xeQueue, string sIncomingQueueName) {
//                // Try to resolve the actual queue name based on whether it's relative, full, or absolute
//                string sReturn = "";
//                if (xeQueue.GetAttribute("type") == "absolute") {
//                    //it's absolute. use the specified queue path
//                    sReturn = sPath;
//                }
//                else {
//                    if ((xeQueue.GetAttribute("type") == "full") || (sIncomingQueueName == "")) {
//                        //it's full. the result queue should be the base queue followed by the specified queue
//                        sReturn = _BaseQueue + sPath;
//                    }
//                    else {
//                        //it's not full and it's not absolute. append the specified queue to the incoming queue
//                        sReturn = sIncomingQueueName + "." + sPath;
//                    }
//                    // If a machine is specified, change the machine in the queue path name
//                    if (xeQueue.GetAttribute("machine") != "") {
//                        sReturn = xeQueue.GetAttribute("machine") + sReturn.Substring(sReturn.IndexOf("\\"));
//                    }
//                }
//                //trim just in case.  messagequeuing will not find a queue with leading spaces and tabs and stuff
//                sReturn = sReturn.Trim();
//                if (!sReturn.ToLower().StartsWith("formatname") && sReturn.ToLower().IndexOf("private$") > -1) {
//                    //doesn't already start with formatname, and it's a private queue
//                    sReturn = "FormatName:DIRECT=OS:" + sReturn;
//                }
//                return sReturn;
//            }
		
//            /// <remarks>
//            /// Once the processing of a message is complete, it needs to be sent to the next
//            /// component via another message queue.  SendApplication() does just that.  The
//            /// message is sent out to each queue as specified by the configuration  At this 
//            /// point a brief description of the queue naming system is merited:
//            /// 
//            /// All queues are by default expected to by Private queues on the local machine.  
//            /// This is set via the _BaseQueue class level variable, which should be done in
//            /// the inherited class configuration method.  The default _BaseQueue is:
//            ///		".\\PRIVATE$\\Aggregator."
//            /// 
//            /// Message queue names consist of a path delineated by "." characters.  Thus,
//            ///		".\\PRIVATE$\\Aggregator.Incoming.Complete.AutoLoan"
//            ///	is a typical queue name.  By default, when outputing to a queue, the name of the
//            ///	queue is determined by apending a new name onto the incoming queue.  Thus, if a
//            ///	step of the process outputs to "Processed" and the incoming queue was the one
//            ///	specified above, then the message would be output to:
//            ///		".\\PRIVATE$\\Aggregator.Incoming.Complete.AutoLoan.Processed"
//            ///	This allows "queue affinity".  Suppose we have some step which is very generic, and
//            ///	executed on several different queues.  This step defines it's output queue as "C"
//            ///	and listens to queues "A" and "B".  With an absolute path, the results would be put
//            ///	out to queue "C".  However, with relative paths, the results go out to either "A.C"
//            ///	or "B.C", thus it's tied to the incoming queue.
//            ///	
//            ///	The same logic applies as the number of incoming and outgoing queues increases.  If 
//            ///	this same process listens to queues "A", "B", and "C" and outputs to queues "D" and 
//            ///	"E", then the results would be output to ("A.D" and "A.E") or ("B.D" and "B.E") or 
//            ///	("C.D" and "C.E").
//            ///	
//            ///	This behavior can be overridden by specifying that the output queue is an absolute 
//            ///	path.
//            /// 
//            /// This routine may be overridden as necessary, though it is not recommended.
//            /// </remarks>
//            protected virtual void SendApplication(XmlDocument Soap, string Header, string IncomingQueueName, XmlNamespaceManager SoapNamespaceManager) {
//                // Gotta look at all nodes so we catch any XSL
//                // Note, this is uber-gay and needs to be done differently.  Maybe XSL in the 
//                // destinations, but that breaks some aspects of the overall schema
//                foreach (XmlElement ChildElement in _ConfigNode.SelectNodes("*")) {
//                    XmlElement xeDestination = ChildElement;
//                    // If this is XSL, transform it and use that as our context
//                    if (xeDestination.Name == "xsl:stylesheet") {
//                        xeDestination = Transform(Soap, ChildElement);
//                    }
//                    if (xeDestination.Name == "destinations") {
//                        SendApplication(Soap, xeDestination, Header, IncomingQueueName, SoapNamespaceManager);
//                    }
//                }
//            }

//            protected virtual void SendApplication(XmlDocument xdApplication, XmlElement xeDestinationNode, string sHeader, string sIncomingQueueName, XmlNamespaceManager xnmNamespaceManager) {
//                // if we're looking at a "destinations" node, we should find info on delivery
//                if (CheckTests(xeDestinationNode, xdApplication, xnmNamespaceManager)) {
//                    //timeout stuff here
//                    TimeSpan tsTimeout = TimeSpan.Zero;
//                    XmlElement xeTimeout = (XmlElement)xeDestinationNode.SelectSingleNode("timeout");
//                    if (xeTimeout != null) {
//                        tsTimeout = new TimeSpan(
//                            int.Parse("0" + GetParameter("days", xeTimeout, xdApplication, xnmNamespaceManager)),
//                            int.Parse("0" + GetParameter("hours", xeTimeout, xdApplication, xnmNamespaceManager)),
//                            int.Parse("0" + GetParameter("minutes", xeTimeout, xdApplication, xnmNamespaceManager)),
//                            int.Parse("0" + GetParameter("seconds", xeTimeout, xdApplication, xnmNamespaceManager)),
//                            int.Parse("0" + GetParameter("milliseconds", xeTimeout, xdApplication, xnmNamespaceManager))
//                            );
//                        XmlElement SoapRouting = xdApplication.CreateElement("soap", "Routing", _SoapURN);
//                        XmlElement SoapLabel   = xdApplication.CreateElement("soap", "Label",   _SoapURN);
//                        SoapLabel.AppendChild(xdApplication.CreateTextNode(sHeader));
//                        SoapRouting.AppendChild(SoapLabel);
//                        xdApplication.SelectSingleNode("soap:Envelope", xnmNamespaceManager).AppendChild(SoapRouting);
//                        foreach (XmlElement DestinationQueue in xeTimeout.SelectNodes("queue")) {
//                            if (CheckTests(DestinationQueue, xdApplication, xnmNamespaceManager)) {
//                                string QueuePath = GetParameter("", DestinationQueue, xdApplication.SelectSingleNode("*"), xnmNamespaceManager);
//                                QueuePath = ResolveQueuePath(QueuePath, DestinationQueue, sIncomingQueueName);
//                                if (QueuePath != "") {
//                                    XmlElement DestinationNode = xdApplication.CreateElement("soap", "Destination", _SoapURN);
//                                    DestinationNode.AppendChild(xdApplication.CreateTextNode(QueuePath));
//                                    SoapRouting.AppendChild(DestinationNode);
//                                }
//                            }
//                        }
//                    }
//                    foreach (XmlElement DestinationQueue in xeDestinationNode.SelectNodes("queue")) {
//                        // Loop over all queues in the destination  All tests on the destinations element
//                        // should have already been performed.
//                        // So get the queue path
//                        string sQueuePath = GetParameter("", DestinationQueue, xdApplication.SelectSingleNode("*"), xnmNamespaceManager).Trim();
//                        // If we actually have a path...
//                        if (sQueuePath != "") {
//                            sQueuePath = ResolveQueuePath(sQueuePath, DestinationQueue, sIncomingQueueName);
//                            // Locate the queue we've settled on
//                            MessageQueue CorrectQueue = null;
//                            try {
//                                CorrectQueue = FindQueue(sQueuePath);
//                                // This would only be null if there's a catastrophic queue creation or naming error
//                                if (CorrectQueue != null) {
//                                    lock (CorrectQueue) {
//                                        try {
//                                            // The Last Queue is useful for running errors back through
//                                            xdApplication.DocumentElement.RemoveAttribute("lastqueue");
//                                            if (!_Silent) {
//                                                xdApplication.DocumentElement.SetAttribute("lastqueue", sQueuePath);
//                                            }
//                                            //handle any syslog stuff
//                                            ProcessSyslogNodes(SyslogTrigger.Send, xdApplication, xnmNamespaceManager);
//                                            // Create and send the message
//                                            Message AppMessage = new Message();
//                                            try {
//                                                //set timeout here
//                                                if (tsTimeout != TimeSpan.Zero) {
//                                                    AppMessage.TimeToBeReceived = tsTimeout;
//                                                    AppMessage.UseDeadLetterQueue = true;
//                                                }
//                                                AppMessage.Body = xdApplication.DocumentElement;
//                                                AppMessage.Label = sHeader;
//                                                AppMessage.Recoverable = true;
//                                                CorrectQueue.Send(AppMessage);
//                                            }
//                                            finally {
//                                                AppMessage.Dispose();
//                                            }
//                                            MonitorSend(MessageType.Application, sIncomingQueueName, sQueuePath, sHeader);
//                                        }
//                                        catch (Exception ex) {
//                                            ReportErrorNow(xdApplication, ex);
//                                        }
//                                    }
//                                }
//                            }
//                            finally {
//                                if (CorrectQueue != null) {
//                                    CorrectQueue.Close();
//                                }
//                            }
//                        }
//                    }
//                }
//            }

//            /// <summary>
//            /// Process the syslog nodes.
//            /// </summary>
//            /// <param name="stAction">The action that triggered the call to ProcessLogNodes.</param>
//            /// <param name="xdApplication">The current application.</param>
//            /// <param name="xnmNamespaceManager">The XmlNamespaceManager for the application.</param>
//            /// <remarks>Only nodes whose "trigger" attributes match stTrigger will be run.</remarks>
//            protected void ProcessSyslogNodes(SyslogTrigger stAction, XmlDocument xdApplication, XmlNamespaceManager xnmNamespaceManager) {
//                try {
//                    foreach (XmlElement xeSyslog in Config.SelectNodes("syslog")) {
//                        try {
//                            SyslogTrigger stTrigger = (SyslogTrigger)Enum.Parse(typeof(SyslogTrigger), xeSyslog.GetAttribute("trigger"), true);
//                            if (stAction == SyslogTrigger.Both || stTrigger == SyslogTrigger.Both || stAction == stTrigger) {
//                                //write to the syslog
//                                string sTag = GetParameter("tag", xeSyslog, xdApplication, xnmNamespaceManager);
//                                string sContent = GetParameter("content", xeSyslog, xdApplication, xnmNamespaceManager);
//                                string sDestination = GetParameter("destination", xeSyslog, xdApplication, xnmNamespaceManager);
//                                bool bSeparateThread = false;
//                                string sSeparateThread = GetParameter("separatethread", xeSyslog, xdApplication, xnmNamespaceManager);
//                                try {
//                                    bSeparateThread = Convert.ToBoolean(sSeparateThread);
//                                }
//                                catch {
//                                    bSeparateThread = sSeparateThread.Trim().ToLower() == "yes";
//                                }
//                                SyslogClient.FacilityType ftFacility = SyslogClient.ToFacility(GetParameter("facility", xeSyslog, xdApplication, xnmNamespaceManager));
//                                SyslogClient.SeverityRatingType srtSeverity = SyslogClient.ToSeverity(GetParameter("severity", xeSyslog, xdApplication, xnmNamespaceManager));
//                                SyslogClient scSyslogClient = new SyslogClient(sDestination, ftFacility, srtSeverity, sTag, sContent);
//                                if (bSeparateThread) {
//                                    Threading.Thread thSyslogThread = new System.Threading.Thread(new Threading.ThreadStart(scSyslogClient.Send));
//                                    thSyslogThread.Start();
//                                }
//                                else {
//                                    scSyslogClient.Send();
//                                }
//                            }
//                        }
//                        catch (Exception e) {
//                            ReportErrorNow(xdApplication, "Error sending syslog message.", e);
//                        }
//                    }
//                }
//                catch (Exception e) {
//                    ReportErrorNow(xdApplication, "Error sending syslog message.", e);
//                }
//            }

//#endregion

//#region MAIN PART HERE

//            /// <summary>
//            /// ProcessApplication() is the heart of the processing of individual messages.  It takes
//            /// the original message along with the appropriate action node from the config file.  It
//            /// uses these to create a third XML document which specifies how to handle the message.
//            /// Finally, it creates an optional XML fragment.  This fragment is then integrated into
//            /// the SOAP message so that the results are available during later processes
//            /// 
//            /// This routine is expected to be overridden.
//            /// </summary>
//            public virtual XmlElement ProcessApplication(XmlDocument SoapMessage, XmlNamespaceManager SoapNamespaceManager, string MessageTitle, XmlElement ActionNode, ref string Actions) {
//                return null;
//            }
			
//            /// <summary>
//            /// This runs GetParameter over all Nodes which match the ParameterName XPath and returns an array of results
//            /// </summary>
//            /// <param name="ParameterName">XPath to search</param>
//            /// <param name="Context">The XML to look at</param>
//            /// <param name="Application">The Soap Message being processed</param>
//            /// <param name="SoapNamespaceManager">The Namespace Manager for any Namespaced XPaths</param>
//            /// <returns></returns>
//            public string[] GetParameters(string ParameterName, XmlNode Context, XmlNode Application, XmlNamespaceManager SoapNamespaceManager) {
//                return GetParameters("", Context.SelectNodes(ParameterName), Application, SoapNamespaceManager);
//            }
			

//            /// <summary>
//            /// This runs GetParameter over all Nodes passed in after following the supplied XPath and returns an array of results
//            /// </summary>
//            /// <param name="ParameterName">Relative XPath to look at</param>
//            /// <param name="Context">All nodes to check</param>
//            /// <param name="Application">The Soap Message being processed</param>
//            /// <param name="SoapNamespaceManager">The Namespace Manager for any Namespaced XPaths</param>
//            /// <returns></returns>
//            public string[] GetParameters(string ParameterName, XmlNodeList Context, XmlNode Application, XmlNamespaceManager SoapNamespaceManager) {
//                string[] Result = new string[Context.Count];
				
//                for (int I = 0; I < Context.Count; I++)
//                    Result[I] = GetParameter(ParameterName, Context[I], Application, SoapNamespaceManager);

//                return Result;
//            }
			
			
//            /// <summary>
//            /// GetParameter() is used pretty widely.  It's a generic "GetString" style routine 
//            /// which builds a string from text data as well as various instructions found in various
//            /// xml files.
//            /// 
//            /// This routine can not be overridden, but should be expanded as necessary while 
//            /// maintaining backwards compatibility to add necessary functionality.  Should a 
//            /// particular tool need custom extensions, these can be added to GetCustomParameter().
//            /// </summary>
//            /// <param name="ParameterName">XPath to search</param>
//            /// <param name="Context">The XML to look at</param>
//            /// <param name="Application">The Soap Message being processed</param>
//            /// <param name="SoapNamespaceManager">The Namespace Manager for any Namespaced XPaths</param>
//            /// <returns></returns>
//            public string GetParameter(string ParameterName, XmlNode Context, XmlNode Application, XmlNamespaceManager SoapNamespaceManager) {
//                string     Result     = "";
//                XmlNode TargetNode = null;								
//                // If a parameter is passed, target that XPath search, otherwise use the current context
//                if (ParameterName != "") {
//                    TargetNode = Context.SelectSingleNode(ParameterName);
//                }
//                else {
//                    TargetNode = Context;
//                }
//                // Found no data so don't bother going on
//                if (TargetNode == null) {
//                    return "";
//                }
//                // Look at all children for data
//                foreach (XmlNode ChildNode in TargetNode.ChildNodes) {
//                    try {
//                        // If this is an element which is (or represents) plain text, just copy that text to the results
//                        if ((ChildNode.NodeType == XmlNodeType.Text) || (ChildNode.NodeType == XmlNodeType.CDATA) || (ChildNode.NodeType == XmlNodeType.EntityReference)) {
//                            Result = Result + ChildNode.InnerText;
//                        }
//                        else if (ChildNode.NodeType == XmlNodeType.Element) {
//                            // Elements all need to be processed
//                            string Format = "";
//                            string ChildText = "";
//                            string XPath = "";
//                            XmlNode XPathResultNode = null;
//                            XmlNodeList TempNodeListResult = null;
//                            XmlElement ChildElement = (XmlElement)ChildNode;
//                            switch (ChildNode.Name) {
//                                    #region xpath
//                                    // This runs an XPath against the Application and gets a single text result
//                                case "xpath": {
//                                    XPath = GetParameter("", ChildElement, Application, SoapNamespaceManager).Trim();
//                                    switch (ChildElement.GetAttribute("index").ToLower()) {
//                                        case "":
//                                        case "first":
//                                        case "1":
//                                            // Optimizations for the first item
//                                            XPathResultNode = Application.SelectSingleNode(XPath, SoapNamespaceManager);
//                                            break;
//                                        case "last":
//                                            // The Last Item
//                                            TempNodeListResult = Application.SelectNodes(XPath, SoapNamespaceManager);
//                                            XPathResultNode = TempNodeListResult[TempNodeListResult.Count - 1];
//                                            break;
//                                        default:
//                                            // This Nth Item
//                                            TempNodeListResult = Application.SelectNodes(XPath, SoapNamespaceManager);
//                                            try {
//                                                int Index = int.Parse(ChildElement.GetAttribute("index"));
//                                                if (Index < 0) Index = TempNodeListResult.Count + Index;
//                                                XPathResultNode = TempNodeListResult[Index];
//                                            }
//                                            catch {
//                                                XPathResultNode = null;
//                                            }
//                                            break;
//                                    }
//                                    if (XPathResultNode != null) {
//                                        Result = Result + XPathResultNode.InnerText;
//                                    }
//                                    break;
//                                }
//                                    #endregion
//                                    #region node
//                                case "node":
//                                    XPath = GetParameter("", ChildElement, Application, SoapNamespaceManager).Trim();
//                                switch (ChildElement.GetAttribute("index").ToLower()) {
//                                    case "":
//                                    case "first":
//                                    case "1":
//                                        XPathResultNode = Application.SelectSingleNode(XPath, SoapNamespaceManager);
//                                        break;
//                                    case "last":
//                                        TempNodeListResult = Application.SelectNodes(XPath, SoapNamespaceManager);
//                                        XPathResultNode = TempNodeListResult[TempNodeListResult.Count - 1];
//                                        break;
//                                    default:
//                                        TempNodeListResult = Application.SelectNodes(XPath, SoapNamespaceManager);
//                                        try {
//                                            XPathResultNode = TempNodeListResult[int.Parse(ChildElement.GetAttribute("index"))];
//                                        }
//                                        catch {
//                                            XPathResultNode = null;
//                                        }
//                                        break;
//                                }

//                                    if (XPathResultNode != null) {
//                                        switch (ChildElement.GetAttribute("method").ToLower()) {
//                                            case "inner":
//                                            case "innerxml":
//                                                Result = Result + XPathResultNode.InnerXml;
//                                                break;
//                                            case "innertext":
//                                                Result = Result + XPathResultNode.InnerText;
//                                                break;
//                                            case "outer":
//                                            case "outerxml":
//                                            default:
//                                                Result = Result + XPathResultNode.OuterXml;
//                                                break;
//                                        }
//                                    }
//                                    break;
//                                    #endregion
//                                    #region count
//                                case "count":
//                                    XPath = GetParameter("", ChildElement, Application, SoapNamespaceManager).Trim();
//                                    Result = Result + Application.SelectNodes(XPath, SoapNamespaceManager).Count;
//                                    break;
//                                    #endregion
//                                    #region machine
//                                case "machine":
//                                    Result = Result + System.Net.Dns.GetHostName();
//                                    break;
//                                    #endregion
//                                    #region hash
//                                case "hash":
//                                    Result = Result + System.Convert.ToBase64String(new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(GetParameter("", ChildElement, Application, SoapNamespaceManager).Trim().ToLower())));
//                                    break;
//                                    #endregion
//                                    #region date
//                                case "date":
//                                    Format = ChildElement.GetAttribute("format");
//                                    if (Format == null || Format == "") Format = "yyyymmdd";
//                                    goto case "datetime";
//                                    #endregion
//                                    #region time
//                                case "time":
//                                    Format = ChildElement.GetAttribute("format");
//                                    if (Format == null || Format == "") Format = "hhnnss";
//                                    goto case "datetime";
//                                    #endregion
//                                    #region datetime
//                                case "datetime":
//                                    if (Format == null || Format == "") Format = ChildElement.GetAttribute("format");
//                                    if (Format == null || Format == "") Format = "yyyymmddhhnnss";
//                                    DateTime TargetDate;
//                                    ChildText = GetParameter("", ChildElement, Application, SoapNamespaceManager);
//                                    try {
//                                        if (ChildText.Length == 14) {
//                                            TargetDate = new DateTime(
//                                                int.Parse(ChildText.Substring(0,  4)), 
//                                                int.Parse(ChildText.Substring(4,  2)),
//                                                int.Parse(ChildText.Substring(6,  2)),
//                                                int.Parse(ChildText.Substring(8,  2)),
//                                                int.Parse(ChildText.Substring(10, 2)),
//                                                int.Parse(ChildText.Substring(12, 2)));
//                                        }
//                                        else {
//                                            TargetDate = DateTime.Parse(ChildText);
//                                        }
//                                    }
//                                    catch {
//                                        if ((ChildText != "") && (ChildText != null)) {
//                                            try {
//                                                TargetDate = new DateTime(int.Parse(ChildText.Substring(0,  4)), 
//                                                    int.Parse(ChildText.Substring(4,  2)),
//                                                    int.Parse(ChildText.Substring(6,  2)),
//                                                    int.Parse(ChildText.Substring(8,  2)),
//                                                    int.Parse(ChildText.Substring(10, 2)),
//                                                    int.Parse(ChildText.Substring(12, 2)));
//                                            }
//                                            catch {
//                                                TargetDate = DateTime.Now;
//                                            }
//                                        }
//                                        else {
//                                            TargetDate = DateTime.Now;
//                                        }
//                                    }
//                                    Format = Format.Replace("yyyy", String.Format("{0:000#}", TargetDate.Year));
//                                    Format = Format.Replace("yy",   String.Format("{0:0#}",   TargetDate.Year % 100));
//                                    Format = Format.Replace("mm",   String.Format("{0:0#}",   TargetDate.Month));
//                                    Format = Format.Replace("dd",   String.Format("{0:0#}",   TargetDate.Day));
//                                    Format = Format.Replace("hh",   String.Format("{0:0#}",   TargetDate.Hour));
//                                    Format = Format.Replace("nn",   String.Format("{0:0#}",   TargetDate.Minute));
//                                    Format = Format.Replace("ss",   String.Format("{0:0#}",   TargetDate.Second));
//                                    Result = Result + Format;
//                                    break;
//                                    #endregion
//                                    #region section
//                                case "section":
//                                    if (CheckTests(ChildElement, Application, SoapNamespaceManager)) {
//                                        string NewContext = GetParameter("context", ChildElement, Application, SoapNamespaceManager);
//                                        if ((NewContext == "") || (Context == null)) {
//                                            Result = Result + GetParameter("", ChildElement, Application, SoapNamespaceManager);
//                                        }
//                                        else {
//                                            XmlDocument TempXml = new XmlDocument();
//                                            TempXml.LoadXml(NewContext);

//                                            Result = Result + GetParameter("", ChildElement, TempXml, null);
//                                        }
//                                    }
//                                    break;
//                                    #endregion
//                                    #region replace
//                                    //								This was a pretty heterosexually challenged element
//                                    //								It was too hard to document, so it was removed
//                                    //
//                                    //								case "replace":
//                                    //								{
//                                    //									Xml.XmlNode TempDoc = Application.CloneNode(true);
//                                    //									XPath = GetParameter("location", ChildElement, Application, SoapNamespaceManager);
//                                    //									string NewText = GetParameter("newdata", ChildElement, Application, SoapNamespaceManager);
//                                    //									string XMLContext = GetParameter("context", ChildElement, Application, SoapNamespaceManager);
//                                    //									string Method = ChildElement.GetAttribute("method");
//                                    //									string Index = ChildElement.GetAttribute("index");
//                                    //									Xml.XmlDocument NewDoc = new Xml.XmlDocument();
//                                    //
//                                    //									Xml.NameTable           ResultSoapNameTable        = new Xml.NameTable();
//                                    //									Xml.XmlNamespaceManager ResultSoapNamespaceManager = new Xml.XmlNamespaceManager(ResultSoapNameTable);
//                                    //			
//                                    //									ResultSoapNamespaceManager.AddNamespace(String.Empty, "urn:none");
//                                    //									ResultSoapNamespaceManager.AddNamespace("soap",       _SoapURN);
//                                    //
//                                    //									Xml.XmlDocument ResultDoc = new Xml.XmlDocument(ResultSoapNameTable);
//                                    //
//                                    //									if (Method.ToLower() == "xml")
//                                    //										NewDoc.LoadXml(NewText);
//                                    //
//                                    //									if ((XMLContext == "") || (XMLContext == null))
//                                    //										ResultDoc.LoadXml(Application.OuterXml);
//                                    //									else
//                                    //										ResultDoc.LoadXml(XMLContext);
//                                    //
//                                    //									switch (Index.ToLower())
//                                    //									{
//                                    //										case "first":
//                                    //										case "1":
//                                    //										{
//                                    //											Xml.XmlNode BadNode = ResultDoc.SelectSingleNode(XPath, SoapNamespaceManager);
//                                    //											if (Method == "xml")
//                                    //												BadNode.ParentNode.ReplaceChild(BadNode.OwnerDocument.ImportNode(NewDoc.DocumentElement, true), BadNode);
//                                    //											else
//                                    //											{
//                                    //												switch (BadNode.NodeType)
//                                    //												{
//                                    //													case Xml.XmlNodeType.Attribute:
//                                    //														((Xml.XmlAttribute)BadNode).Value = NewText;
//                                    //														break;
//                                    //													default:
//                                    //														foreach (Xml.XmlNode BadChild in BadNode.ChildNodes)
//                                    //															BadNode.RemoveChild(BadChild);
//                                    //														BadNode.AppendChild(BadNode.OwnerDocument.CreateTextNode(NewText));
//                                    //														break;
//                                    //												}
//                                    //											}
//                                    //
//                                    //											break;
//                                    //										}
//                                    //										case "all":
//                                    //										case "*":
//                                    //										case "":
//                                    //										{
//                                    //											/*
//                                    //												if (Method == "xml")
//                                    //													Application.SelectSingleNode(XPath, SoapNamespaceManager).InnerXml = NewText;
//                                    //												else
//                                    //													Application.SelectSingleNode(XPath, SoapNamespaceManager).InnerText = NewText;
//                                    //												*/
//                                    //											break;
//                                    //										}
//                                    //									}
//                                    //
//                                    //									Result = Result + TempDoc.OuterXml;
//                                    //
//                                    //									break;
//                                    //								}
//                                    #endregion
//                                    #region text
//                                case "text":
//                                    Result = Result + GetParameter("", ChildElement, Application, SoapNamespaceManager);
//                                    break;
//                                    #endregion
//                                    #region format
//                                case "format":
//                                    Result = Result + String.Format(ChildElement.GetAttribute("spec"), GetParameter("", ChildElement, Application, SoapNamespaceManager));
//                                    break;
//                                    #endregion
//                                    #region literal
//                                case "literal":
//                                    Result = Result + ChildElement.InnerXml;
//                                    break;
//                                    #endregion
//                                    #region encode
//                                case "encode": {
//                                    switch (ChildElement.GetAttribute("method").ToLower()) {
//                                        case "":
//                                        case "url":
//                                            Result = Result + System.Web.HttpUtility.UrlEncode(GetParameter("", ChildElement, Application, SoapNamespaceManager));
//                                            break;
//                                        case "html":
//                                        case "xml":
//                                            Result = Result + System.Web.HttpUtility.HtmlEncode(GetParameter("", ChildElement, Application, SoapNamespaceManager));
//                                            break;
//                                        case "attribute":
//                                        case "att":
//                                            Result = Result + System.Web.HttpUtility.HtmlAttributeEncode(GetParameter("", ChildElement, Application, SoapNamespaceManager));
//                                            break;
//                                        case "base64":
//                                            Result = Result + System.Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(GetParameter("", ChildElement, Application, SoapNamespaceManager)));
//                                            break;
//                                        case "qp":
//                                        case "quoted-printable":
//                                            Result += QuotedPrintable.Encode(GetParameter("", ChildElement, Application, SoapNamespaceManager));
//                                            break;
//                                        case "base32": {
//                                            Result += Base32Encoder.Encode(System.Text.ASCIIEncoding.ASCII.GetBytes(GetParameter("", ChildElement, Application, SoapNamespaceManager)));
//                                            break;
//                                        }
//                                        default:
//                                            Result = Result + "Not Supported";
//                                            break;
//                                    }
//                                    break;
//                                }
//                                    #endregion
//                                    #region decode
//                                case "decode": {
//                                    switch (ChildElement.GetAttribute("method")) {
//                                        case "":
//                                        case "url":
//                                            Result = Result + System.Web.HttpUtility.UrlDecode(GetParameter("", ChildElement, Application, SoapNamespaceManager));
//                                            break;
//                                        case "html":
//                                        case "xml":
//                                        case "attribute":
//                                        case "att":
//                                            Result = Result + System.Web.HttpUtility.HtmlDecode(GetParameter("", ChildElement, Application, SoapNamespaceManager));
//                                            break;
//                                        case "base64":
//                                            Result = Result + System.Text.Encoding.ASCII.GetString(System.Convert.FromBase64String(GetParameter("", ChildElement, Application, SoapNamespaceManager)));
//                                            break;
//                                        case "base32": {
//                                            Result += System.Text.Encoding.ASCII.GetString(Base32Encoder.Decode(GetParameter("", ChildElement, Application, SoapNamespaceManager)));
//                                            break;
//                                        }
//                                        default:
//                                            Result = Result + "Not Supported";
//                                            break;
//                                    }
//                                    break;
//                                }
//                                    #endregion
//                                    #region regex
//                                case "regex":
//                                    string MatchSpec = ChildElement.GetAttribute("match");
//                                    if (ChildElement.GetAttributeNode("replace") != null) {
//                                        Result = Result + Regex.Regex.Replace(GetParameter("", ChildElement, Application, SoapNamespaceManager), MatchSpec, ChildElement.GetAttribute("replace"));
//                                    }
//                                    else {
//                                        string sMatchData = GetParameter("", ChildElement, Application, SoapNamespaceManager);
//                                        Regex.Match RegexMatches = Regex.Regex.Match(sMatchData, MatchSpec);
//                                        if (RegexMatches.Success) {
//                                            if (ChildElement.GetAttributeNode("capture") != null) {
//                                                Result = Result + RegexMatches.Groups[ChildElement.GetAttribute("capture")];
//                                            }
//                                            else {
//                                                Result = Result + RegexMatches.Value;
//                                            }
//                                        }
//                                    }
//                                    break;
//                                    #endregion
//                                    #region switch/select
//                                case "switch":
//                                case "select": {
//                                    XmlNode OldDataNode = ChildElement.SelectSingleNode("value");
//                                    string OldData = GetParameter("value", ChildElement, Application, SoapNamespaceManager).ToLower();
//                                    string NewResult = GetParameter("else",  ChildElement, Application, SoapNamespaceManager);
//                                    XmlNodeList Pairs = ChildElement.SelectNodes("case");
//                                    foreach (XmlElement Pair in Pairs) {
//                                        foreach (XmlElement When in Pair.SelectNodes("when")) {
//                                            if (OldDataNode == null) {
//                                                if (CheckTests(When, Application, SoapNamespaceManager)) {
//                                                    NewResult = GetParameter("then", Pair, Application, SoapNamespaceManager);
//                                                    goto FoundResult;
//                                                }
//                                            }
//                                            else {
//                                                string PotentialMatch = GetParameter("", When, Application, SoapNamespaceManager).ToLower();
//                                                if (PotentialMatch == OldData) {
//                                                    NewResult = GetParameter("then", Pair, Application, SoapNamespaceManager);
//                                                    goto FoundResult;
//                                                }
//                                            }
//                                        }
//                                    }

//                                    FoundResult:

//                                        Result = Result + NewResult;

//                                    break;
//                                }
//                                    #endregion
//                                    #region registry
//                                case "registry":
//                                    string HiveName = GetParameter("hive", ChildElement, Application, SoapNamespaceManager);
//                                    string Path     = GetParameter("path", ChildElement, Application, SoapNamespaceManager);
//                                    string RegKey   = GetParameter("key",  ChildElement, Application, SoapNamespaceManager);

//                                    Microsoft.Win32.RegistryKey Hive = null;
//                                switch (HiveName.ToUpper()) {
//                                    case "HKEY_CLASSES_ROOT":
//                                        Hive = Microsoft.Win32.Registry.ClassesRoot;
//                                        break;
//                                    case "HKEY_CURRENT_CONFIG":
//                                        Hive = Microsoft.Win32.Registry.CurrentConfig;
//                                        break;
//                                    case "HKEY_CURRENT_USER":
//                                        Hive = Microsoft.Win32.Registry.CurrentUser;
//                                        break;
//                                    case "HKEY_DYN_DATA":
//                                        Hive = Microsoft.Win32.Registry.DynData;
//                                        break;
//                                    case "HKEY_LOCAL_MACHINE":
//                                        Hive = Microsoft.Win32.Registry.LocalMachine;
//                                        break;
//                                    case "HKEY_PERFORMANCE_DATA":
//                                        Hive = Microsoft.Win32.Registry.PerformanceData;
//                                        break;
//                                    case "HKEY_USERS":
//                                        Hive = Microsoft.Win32.Registry.Users;
//                                        break;
//                                }

//                                    Microsoft.Win32.RegistryKey TargetKey = Hive.OpenSubKey(Path);

//                                    if (TargetKey != null) {
//                                        Result = Result + (string)TargetKey.GetValue(RegKey);
//                                    }
//                                    break;
//                                    #endregion
//                                    #region xsl:stylesheet
//                                case "xsl:stylesheet":
//                                    Result = Result + GetParameter("", Transform((XmlDocument)Application, ChildElement), Application, SoapNamespaceManager);
//                                    break;
//                                    #endregion
//                                    #region debug
//                                case "debug":
//                                    string NewSegment = GetParameter("", ChildElement, Application, SoapNamespaceManager);
//                                    Result = Result + NewSegment;
//                                    Debug(ChildElement.GetAttribute("label"), NewSegment, DebugMessageType.User, ChildElement.GetAttribute("debugqueue"));
//                                    if (ChildElement.GetAttribute("label") != "") {
//                                        System.Console.Write(ChildElement.GetAttribute("label") + ": ");
//                                        System.Diagnostics.Debug.Write(ChildElement.GetAttribute("label") + ": ");
//                                    }
//                                    System.Console.WriteLine(NewSegment);
//                                    System.Diagnostics.Debug.WriteLine(NewSegment);
//                                    break;
//                                    #endregion
//                                    #region encrypt
//                                case "encrypt": {
//                                    byte[] Data = System.Text.ASCIIEncoding.ASCII.GetBytes(GetParameter("", ChildElement, Application, SoapNamespaceManager));

//                                    if (Data.Length > 0) {
//                                        Cryptography.SymmetricAlgorithm Provider = null;

//                                        switch (ChildElement.GetAttribute("method").ToLower()) {
//                                            case "rc2":
//                                                Provider = new Cryptography.RC2CryptoServiceProvider();
//                                                break;
//                                            case "rijndael":
//                                                Provider = new Cryptography.RijndaelManaged();
//                                                break;
//                                            default:
//                                            case "3des":
//                                            case "tripledes":
//                                                Provider = new Cryptography.TripleDESCryptoServiceProvider();
//                                                break;
//                                            case "des":
//                                                Provider = new Cryptography.DESCryptoServiceProvider();
//                                                break;
//                                        }

//                                        string Key = ChildElement.GetAttribute("key");
//                                        string IV  = ChildElement.GetAttribute("iv");

//                                        if (Key == "") Key = _DefaultEncryptionKey;

//                                        if (ChildElement.GetAttribute("encoding").ToLower() == "base64") {
//                                            Provider.Key = System.Convert.FromBase64String(Key);
//                                            if (IV != "")
//                                                Provider.IV  = System.Convert.FromBase64String(IV);
//                                        }
//                                        else {
//                                            Provider.Key = System.Text.ASCIIEncoding.ASCII.GetBytes(Key);
//                                            Provider.IV  = System.Text.ASCIIEncoding.ASCII.GetBytes(IV);
//                                        }

//                                        System.IO.MemoryStream DataStream = new System.IO.MemoryStream();
//                                        Cryptography.CryptoStream EncryptionStream = new System.Security.Cryptography.CryptoStream(DataStream, Provider.CreateEncryptor(), Cryptography.CryptoStreamMode.Write);

//                                        EncryptionStream.Write(Data, 0, Data.Length);
//                                        EncryptionStream.FlushFinalBlock();
											
//                                        DataStream.Seek(0, System.IO.SeekOrigin.Begin);

//                                        byte[] DataBuffer = new byte[DataStream.Length];
//                                        DataStream.Read(DataBuffer, 0, (int)DataStream.Length);

//                                        Result = Result + System.Convert.ToBase64String(DataBuffer);
//                                    }

//                                    break;
//                                }
//                                    #endregion
//                                    #region decrypt
//                                case "decrypt": {
//                                    byte[] Data = System.Convert.FromBase64String(GetParameter("", ChildElement, Application, SoapNamespaceManager));

//                                    if (Data.Length > 0) {
//                                        Cryptography.SymmetricAlgorithm Provider = null;

//                                        switch (ChildElement.GetAttribute("method").ToLower()) {
//                                            case "rc2":
//                                                Provider = new Cryptography.RC2CryptoServiceProvider();
//                                                break;
//                                            case "rijndael":
//                                                Provider = new Cryptography.RijndaelManaged();
//                                                break;
//                                            default:
//                                            case "3des":
//                                            case "tripledes":
//                                                Provider = new Cryptography.TripleDESCryptoServiceProvider();
//                                                break;
//                                            case "des":
//                                                Provider = new Cryptography.DESCryptoServiceProvider();
//                                                break;
//                                        }

//                                        string Key = ChildElement.GetAttribute("key");
//                                        string IV  = ChildElement.GetAttribute("iv");

//                                        if (Key == "") Key = _DefaultEncryptionKey;

//                                        if (ChildElement.GetAttribute("encoding").ToLower() == "base64") {
//                                            Provider.Key = System.Convert.FromBase64String(Key);
//                                            if (IV != "")
//                                                Provider.IV  = System.Convert.FromBase64String(IV);
//                                        }
//                                        else {
//                                            Provider.Key = System.Text.ASCIIEncoding.ASCII.GetBytes(Key);
//                                            Provider.IV  = System.Text.ASCIIEncoding.ASCII.GetBytes(IV);
//                                        }

//                                        System.IO.MemoryStream DataStream = new System.IO.MemoryStream();
//                                        Cryptography.CryptoStream DecryptionStream = new System.Security.Cryptography.CryptoStream(DataStream, Provider.CreateDecryptor(), Cryptography.CryptoStreamMode.Write);

//                                        DecryptionStream.Write(Data, 0, Data.Length);
//                                        DecryptionStream.FlushFinalBlock();
											
//                                        DataStream.Seek(0, System.IO.SeekOrigin.Begin);

//                                        byte[] DataBuffer = new byte[DataStream.Length];
//                                        DataStream.Read(DataBuffer, 0, (int)DataStream.Length);

//                                        Result = Result + System.Text.ASCIIEncoding.ASCII.GetString(DataBuffer);
//                                    }

//                                    break;
//                                }
//                                    #endregion
//                                    #region code
//                                case "code": {
//                                    Compilers.ICodeEngine Compiler = null;
//                                    switch (ChildElement.GetAttribute("language").ToLower()) {
//                                        case "vb":
//                                        case "vb.net":
//                                        case "visualbasic":
//                                        case "visual basic":
//                                            Compiler = new Compilers.VisualBasic();
//                                            break;
//                                        case "":
//                                        case "cs":
//                                        case "c#":
//                                        case "csharp":
//                                        case "c-sharp":
//                                            Compiler = new Compilers.CSharp();
//                                            break;
//                                        default:
//                                            ReportError(Application.OwnerDocument, "GetParameter() - 'code' Element", "Language '" + ChildElement.GetAttribute("language") + "' not recognized");
//                                            break;
//                                    }
//                                    string[] Imports = new string[ChildElement.SelectNodes("assembly").Count + 1];
//                                    int I = 0;
//                                    foreach (XmlElement AssemblyNode in ChildElement.SelectNodes("assembly")) {
//                                        Imports[I++] = GetParameter("", AssemblyNode, Application, SoapNamespaceManager);
//                                    }
//                                    Imports[I] =  "System.Xml.dll";
//                                    string Code = GetParameter("source", ChildElement, Application, SoapNamespaceManager);
//                                    int CodeHash = Code.GetHashCode();
//                                    System.Reflection.Assembly TempAssembly = Compiler.Compile(Code, true, Imports);
//                                    string ClassName = ChildElement.GetAttribute("class");
//                                    if (ChildElement.GetAttribute("class") == "") {
//                                        if (TempAssembly.GetTypes().Length == 1) {
//                                            ClassName = TempAssembly.GetTypes()[0].FullName;
//                                        }
//                                        else {
//                                            ReportError(Application.OwnerDocument, "GetParameter() - 'code' Element", "Ambiguous Which Class Should Be Used");
//                                        }
//                                    }
//                                    System.Type TempType = TempAssembly.GetType(ClassName);
//                                    System.Reflection.ConstructorInfo CI = TempType.GetConstructor(new Type[] {typeof(XmlDocument), typeof(XmlNamespaceManager)});	
//                                    if (CI != null) {
//                                        object Temp = CI.Invoke(System.Reflection.BindingFlags.InvokeMethod, null, new object[2] {Application.OwnerDocument, SoapNamespaceManager}, null);
//                                        Result += Temp.ToString();
//                                    }
//                                    else {
//                                        CI = TempType.GetConstructor(Type.EmptyTypes);
//                                        if (CI != null) {
//                                            Result += CI.Invoke(System.Reflection.BindingFlags.InvokeMethod, null, null, null).ToString();
//                                        }
//                                        else {
//                                            ReportErrorNow((Application is XmlDocument ? (XmlDocument)Application : (Application != null ? Application.OwnerDocument : null)), "Appropriate Constructor Not Found", "No Appropriate Constructor Found in '" + ClassName + "'");
//                                        }
//                                    }
//                                    break;
//                                }
//                                    #endregion
//                                    #region guid
//                                case "guid":
//                                    if (ChildElement.GetAttribute("type").ToLower() == "full")
//                                        Result = Result + System.Guid.NewGuid().ToString();
//                                    else
//                                        Result = Result + NewGuid;
//                                    break;
//                                    #endregion
//                                    #region evaluate
//                                case "evaluate": {
//                                    XPath = GetParameter("", ChildElement, Application, SoapNamespaceManager).Trim();
//                                    switch (ChildElement.GetAttribute("index").ToLower()) {
//                                        case "":
//                                        case "first":
//                                        case "1":
//                                            XPathResultNode = Application.SelectSingleNode(XPath, SoapNamespaceManager);
//                                            break;
//                                        case "last":
//                                            TempNodeListResult = Application.SelectNodes(XPath, SoapNamespaceManager);
//                                            XPathResultNode = TempNodeListResult[TempNodeListResult.Count - 1];
//                                            break;
//                                        default:
//                                            TempNodeListResult = Application.SelectNodes(XPath, SoapNamespaceManager);
//                                            try {
//                                                XPathResultNode = TempNodeListResult[int.Parse(ChildElement.GetAttribute("index"))];
//                                            }
//                                            catch {
//                                                XPathResultNode = null;
//                                            }
//                                            break;
//                                    }
//                                    if (XPathResultNode != null) {
//                                        Result = Result + GetParameter("", XPathResultNode, Application, SoapNamespaceManager);
//                                    }
//                                    break;
//                                }
//                                    #endregion
//                                    #region validate
//                                    /*
//                                     * <validate> validates its innards for well-formedness or against a DTD or schema.
//                                     * if the validation passes, the evaluated insides get passed right along
//                                     * if the validation fails, an exception is thrown.
//                                     *	type should be "wellformed", "schema", or "dtd". defaults to "wellformed".
//                                     *	href is the schema or dtd location 
//                                     *	for schema validation, the default namespace in the xml fragment will be the targetNamespace of the schema
//                                     *	for dtd validation, the dtd declaration is prepended to the xml fragment, and the root element is the doctype identifier
//                                     * example:
//                                     *	<!-- dtd validation -->
//                                     *		<validate type="dtd" href="c:\testes.dtd">
//                                     *			<literal>
//                                     *				<!-- comment to mess things up -->
//                                     *				<testes name="yo">
//                                     *					<subtestes>hello</subtestes>
//                                     *				</testes>
//                                     *			</literal>
//                                     *		</validate>
//                                     *	the actual xml that will be validated is:
//                                     *		<!DOCTYPE testes SYSTEM 'c:\testes.dtd'>
//                                     *		<testes name="yo">
//                                     *			<subtestes>hello</subtestes>
//                                     *		</testes>
//                                     * 
//                                     *	<!-- schema validation -->
//                                     *		<validate type="schema" href="c:\testes.xsd">
//                                     *			<literal>
//                                     *				<!-- comment to mess things up -->
//                                     *				<testes name="yo">
//                                     *					<subtestes>hello</subtestes>
//                                     *				</testes>
//                                     *			</literal>
//                                     *		</validate>
//                                     *	the actual xml that will be validated is:
//                                     *		<testes name="yo" xmlns="(the targetNamespace from c:\testes.xsd)">
//                                     *			<subtestes>hello</subtestes>
//                                     *		</testes>
//                                    */
//                                case "validate": {
//                                    string sValidationType = ChildElement.GetAttribute("type").ToLower().Trim();
//                                    if (sValidationType.Length == 0) {
//                                        sValidationType = "wellformed";
//                                    }
//                                    string sValidationDocument = ChildElement.GetAttribute("href").Trim();
//                                    string sXml = GetParameter("", ChildElement, Application, SoapNamespaceManager).Trim();
//                                    XmlValidator xvValidator = new XmlValidator(sXml, sValidationType, sValidationDocument);
//                                    switch (xvValidator.ValidationType) {
//                                        case XmlValidator.XmlValidationType.WellFormed: {
//                                            //don't need to do anything
//                                            break;
//                                        }
//                                        case XmlValidator.XmlValidationType.Schema: {
//                                            //still don't need to do anything
//                                            break;
//                                        }
//                                        case XmlValidator.XmlValidationType.DTD: {
//                                            break;
//                                        }
//                                    }
//                                    if (xvValidator.IsValid) {
//                                        Result += sXml;
//                                    }
//                                    else {
//                                        throw new Exception("The XML fragment did not pass validation.\n" + xvValidator.ErrorMessages);
//                                    }
//                                    break;
//                                }
//                                    #endregion
//                                    #region weekofyear
//                                case "weekofyear": {
//                                    string sContents = GetParameter("", ChildElement, Application, SoapNamespaceManager);
//                                    DateTime dtDate;
//                                    try {
//                                        dtDate = Convert.ToDateTime(sContents);
//                                    }
//                                    catch (Exception e) {
//                                        //if there is a problem parsing the date, default to today
//                                        System.Diagnostics.Debug.WriteLine(e.ToString());
//                                        dtDate = DateTime.Today;
//                                    }
//                                    int iWeekOfYear = CultureInfo.CurrentCulture.DateTimeFormat.Calendar.GetWeekOfYear(dtDate, CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule, CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);
//                                    Result += iWeekOfYear.ToString();
//                                    break;
//                                }
//                                    #endregion
//                                    #region weekofmonth
//                                case "weekofmonth": {
//                                    string sContents = GetParameter("", ChildElement, Application, SoapNamespaceManager);
//                                    DateTime dtDate;
//                                    try {
//                                        dtDate = Convert.ToDateTime(sContents);
//                                    }
//                                    catch (Exception e) {
//                                        //if there is a problem parsing the date, default to today
//                                        System.Diagnostics.Debug.WriteLine(e.ToString());
//                                        dtDate = DateTime.Today;
//                                    }
//                                    int iWeekOfMonth = (dtDate.Day / 7) + (dtDate.Day % 7 == 0 ? 0 : 1);
//                                    Result += iWeekOfMonth.ToString();
//                                    break;
//                                }
//                                    #endregion
//                                    #region transform
//                                case "transform": {
//                                    string sXslFilename = "";
//                                    string sXml = "";
//                                    XmlAttribute xaXslFilename = (XmlAttribute)ChildNode.SelectSingleNode("@xslfilename", SoapNamespaceManager);
//                                    if (xaXslFilename != null) {
//                                        sXslFilename = xaXslFilename.Value.Trim();
//                                        sXml = GetParameter("", ChildNode, Application, SoapNamespaceManager);
//                                    }
//                                    else {
//                                        sXslFilename = GetParameter("xslfilename", ChildNode, Application, SoapNamespaceManager);
//                                        sXml = GetParameter("content", ChildNode, Application, SoapNamespaceManager);
//                                    }
//                                    //get the full path to the filename
//                                    if (sXslFilename.IndexOf("://") == -1 && sXslFilename.Substring(1, 1) != ":") {
//                                        RegistryKey rkConfig = Registry.LocalMachine.OpenSubKey(MessageQueueSpawner._RegistryKeyConfigFilePath);
//                                        if (rkConfig != null) {
//                                            sXslFilename = System.IO.Path.Combine(Convert.ToString(rkConfig.GetValue("ConfigPath")), sXslFilename);
//                                        }
//                                    }
//                                    //the XsltArgumentList is for xslt extension functions
//                                    XsltArgumentList xalArguments = new XsltArgumentList();
//                                    xalArguments.AddExtensionObject("http://www.1800communications.com/schemas", new XsltExtensionFunctions());
//                                    XslTransform xslTransform = new XslTransform();
//                                    xslTransform.Load(sXslFilename, new XmlUrlResolver());
//                                    XmlDocument xdToTransform = new XmlDocument();
//                                    xdToTransform.LoadXml(sXml);
//                                    StringWriter swResults = new StringWriter();
//                                    xslTransform.Transform(xdToTransform, xalArguments, swResults, null);
//                                    Result += swResults.ToString();
//                                    break;
//                                }
//                                    #endregion
//                                    #region random
//                                case "random": {
//                                    XmlElement xeChildNode = (XmlElement)ChildNode;
//                                    string sMin = xeChildNode.GetAttribute("min");
//                                    string sMax = xeChildNode.GetAttribute("max");
//                                    int iRandom = 0;
//                                    if (sMax.Length > 0) {
//                                        if (sMin.Length > 0) {
//                                            iRandom = m_rndRandomNumberGenerator.Next(Convert.ToInt32(sMin), Convert.ToInt32(sMax));
//                                        }
//                                        else {
//                                            iRandom = m_rndRandomNumberGenerator.Next(Convert.ToInt32(sMax));
//                                        }
//                                    }
//                                    else {
//                                        iRandom = m_rndRandomNumberGenerator.Next();
//                                    }
//                                    Result += iRandom.ToString();
//                                    break;
//                                }
//                                    #endregion
//                                    #region queuesize
//                                case "queuesize": {
//                                    string sQueuePath = GetParameter("", ChildElement, Application, SoapNamespaceManager);
//                                    sQueuePath = ResolveQueuePath(sQueuePath, ChildElement, m_sIncomingQueuePath);
//                                    long lMessageCount = 0;
//                                    try {
//                                        lMessageCount = QueueMonitorClient.MessageCount(sQueuePath);
//                                    }
//                                    catch (Exception e) {
//                                        System.Diagnostics.Debug.WriteLine(e.ToString());
//                                    }
//                                    Result += Convert.ToString(lMessageCount);
//                                    break;
//                                }
//                                    #endregion
//                                    #region validemail
//                                case "validemail": {
//                                    string sEmail = GetParameter("", ChildElement, Application, SoapNamespaceManager);
//                                    //get the timeout
//                                    int iTimeout = 60000;
//                                    if (ChildElement.HasAttribute("timeout")) {
//                                        try {
//                                            iTimeout = Convert.ToInt32(ChildElement.GetAttribute("timeout"));
//                                        }
//                                        catch (Exception e) {
//                                            System.Diagnostics.Debug.WriteLine(e.ToString());
//                                        }
//                                    }
//                                    //get the target validation level
//                                    HexValidEmailLevel hvelTargetLevel = HexValidEmailLevel.hexVeLevelSmtp;
//                                    if (ChildElement.HasAttribute("level")) {
//                                        switch (ChildElement.GetAttribute("level")) {
//                                            case "smtp": {
//                                                hvelTargetLevel = HexValidEmailLevel.hexVeLevelSmtp;
//                                                break;
//                                            }
//                                            case "dns": {
//                                                hvelTargetLevel = HexValidEmailLevel.hexVeLevelDns;
//                                                break;
//                                            }
//                                            case "syntax": {
//                                                hvelTargetLevel = HexValidEmailLevel.hexVeLevelSyntax;
//                                                break;
//                                            }
//                                        }
//                                    }
//                                    HexValidEmailLib.Connection hcConnection = new HexValidEmailLib.ConnectionClass();
//                                    hcConnection.FromDomain = "1800communications.com";
//                                    hcConnection.FromEmail = "sdallamura@1800communications.com";
//                                    hcConnection.Timeouts.Item(HexValidEmailTimeout.hexVeTimeoutSmtpTotal).Value = iTimeout;
//                                    HexValidEmailLevel hvelResult = (HexValidEmailLevel)hcConnection.Validate(sEmail, hvelTargetLevel);
//                                    if (hvelResult == hvelTargetLevel) {
//                                        Result += "true";
//                                    }
//                                    else {
//                                        System.Diagnostics.Debug.WriteLine(((HexValidEmailErrors)hcConnection.Error).ToString());
//                                        Result += "false";
//                                    }
//                                    break;
//                                }
//                                    #endregion
//                                    #region [Custom Parameters]
//                                default:
//                                    Result = Result + GetCustomParameter((XmlElement)ChildNode, Application, SoapNamespaceManager);
//                                    break;
//                                    #endregion
//                            }
//                        }
//                    }
//                    catch (Compilers.CompilationException cexx) {
//                        // Compilation of the Code Block Jacked Up
//                        string[] Errors = new string[cexx.Errors.Count];
//                        int I = 0;
//                        foreach (System.CodeDom.Compiler.CompilerError Error in cexx.Errors) {
//                            Errors[I++] = Error.ErrorText;
//                        }
//                        ReportErrorNow((Application is XmlDocument ? (XmlDocument)Application : (Application != null ? Application.OwnerDocument : null)), cexx.GetType().ToString(), "Errors During Compilation", null, Errors);
//                    }
//                    catch (Exception exx) {
//                        //Report any other error we found and keep going
//                        //Application can be an XmlDocument. if it is, it has no OwnerDocument. so check.
//                        ReportErrorNow((Application is XmlDocument ? (XmlDocument)Application : (Application != null ? Application.OwnerDocument : null)), exx);
//                    }
//                }

//                return Result;
//            }

//            /// <summary>
//            /// This routine may be overridden by any class inheriting the MessageQueueListener
//            /// class.  The idea is to provide additional functionality to that defined in the
//            /// GetParameter() routine.  For example, the Emailer can understand an "attachment"
//            /// element which attaches a file and returns the ID assigned to that attachment.
//            /// This is useful when creating HTML emails which display images which are attached.
//            /// </summary>
//            /// <param name="UnrecognizedElement">The element which is not recognized</param>
//            /// <param name="Application">The user's application</param>
//            /// <param name="SoapNamespaceManager">The Namespace Manager for the user's Application</param>
//            /// <returns>The string represented by the unrecognized parameter</returns>
//            public virtual string GetCustomParameter(XmlElement UnrecognizedElement, XmlNode Application, XmlNamespaceManager SoapNamespaceManager) {
//                // Default functionality, return a blank string for unrecognized elements
//                return "";
//            }

//#endregion

//            /// <summary>
//            /// This determines whether the designer intended for a given piece
//            /// of the xml to be read depending on various factors including data
//            /// in the user's application.
//            /// </summary>
//            /// <param name="Context">The node whose processing is in question</param>
//            /// <param name="Application">The entire user's application</param>
//            /// <param name="SoapNamespaceManager">
//            ///		Namespace manager for users application
//            /// </param>
//            /// <returns>
//            ///		Returns a boolean determing whether to ignore the node or not
//            ///	</returns>
//            protected bool CheckTests(XmlNode Context, XmlNode Application, XmlNamespaceManager SoapNamespaceManager) {
//                // If we've got an operator...
//                if (((XmlElement)Context).GetAttribute("operator") != "") {
//                    string NextValue = null;

//                    object LastObject = null;
//                    object NextObject = null;

//                    string DataTypeName = null;

//                    XmlNodeList OperandSet = Context.SelectNodes("operand");

//                    switch (((XmlElement)Context).GetAttribute("operator").ToLower()) {
//                        case "equals":
//                        case "==":
//                        case "=":
//                        case "eq":
//                        case "e":
//                            // This is just a normal equals comparison.  Use a default datatype 
//                            // of system.string and use the regular comparison code

//                            DataTypeName = "system.string";
//                            goto default;
//                        case "exists":
//                        case "exist":
//                        case "?":
//                            // This just tests that each operand is non-blank

//                            foreach (XmlElement NextOperand in OperandSet) {
//                                NextValue = GetParameter("", NextOperand, Application, SoapNamespaceManager);
//                                if (NextValue == "") return false;
//                            }

//                            return true;
//                        default: {
//                            // For whatever comparison, we may need to do something very tricky
//                            // Hopefully it's a string comparison.  If it's not a string and
//                            // the object has no Parse(string) method, we're screwed

//                            // Get the desired type (if specified).  If not use the default
//                            // of system.double (note: the default is set to system.string
//                            // for equality comparisons up above).
//                            if (((XmlElement)Context).GetAttributeNode("type") != null) {
//                                DataTypeName = ((XmlElement)Context).GetAttribute("type");
//                            }
//                            if (DataTypeName == null) {
//                                DataTypeName = "system.double";
//                            }
//                            if (DataTypeName.IndexOf(".") < 0) {
//                                DataTypeName = "System." + DataTypeName;
//                            }
//                            // Try to find the Parse(string) and Comparison(DataType) methods
//                            Type DataType = Type.GetType(DataTypeName, true, true);
//                            System.Reflection.MethodInfo ParseMethod      = DataType.GetMethod("Parse",new Type[] {Type.GetType("System.String")});
//                            System.Reflection.MethodInfo ComparisonMethod = DataType.GetMethod("CompareTo", new Type[]{DataType});
							
//                            // Look at each operand after the first and...
//                            foreach (XmlElement NextOperand in OperandSet) {
//                                NextValue = GetParameter("", NextOperand, Application, SoapNamespaceManager).ToLower();
								
//                                // If we can't parse it, try to cast it
//                                try {
//                                    if (ParseMethod != null) {
//                                        NextObject = ParseMethod.Invoke(null, new object[] {NextValue});
//                                    }
//                                    else {
//                                        NextObject = NextValue;
//                                    }
//                                }
//                                catch {
//                                    return false;
//                                }

//                                // If nothing could be done then "Oh Well", but if we can...

//                                if (LastObject != null) {
//                                    // Try to run the comparison routine
//                                    // < 0 : Less than
//                                    // > 0 : Greater than
//                                    // = 0 : Equal to
//                                    int CompareResult = (int)ComparisonMethod.Invoke(LastObject, new object[] {NextObject});
										

//                                    // If the situation is ever false, the whole run will be false, so junk out.
//                                    switch (((XmlElement)Context).GetAttribute("operator").ToLower()) {
//                                        case "equals":
//                                        case "==":
//                                        case "=":
//                                        case "eq":
//                                        case "e":
//                                            if (CompareResult != 0)
//                                                return false;
//                                            break;
//                                        case "greater-than":
//                                        case "greaterthan":
//                                        case "greater":
//                                        case "gt":
//                                        case ">":
//                                            if (CompareResult <= 0)
//                                                return false;
//                                            break;
//                                        case "less-than":
//                                        case "lessthan":
//                                        case "less":
//                                        case "lt":
//                                        case "<":
//                                            if (CompareResult >= 0)
//                                                return false;
//                                            break;
//                                        case "less-than-or-equal":
//                                        case "lessthanorequal":
//                                        case "lessequal":
//                                        case "lte":
//                                        case "le":
//                                        case "<=":
//                                            if (CompareResult > 0)
//                                                return false;
//                                            break;
//                                        case "greater-than-or-equal":
//                                        case "greaterthanorequal":
//                                        case "greaterequal":
//                                        case "gte":
//                                        case "ge":
//                                        case ">=":
//                                            if (CompareResult < 0)
//                                                return false;
//                                            break;
//                                        default:
//                                            ReportError(Application.OwnerDocument, "Comparison Operator not Recognized");
//                                            break;
//                                    }
//                                }

//                                LastObject = NextObject;
//                            }

//                            // If nothing failed, then the test is true
//                            return true;
//                        }
//                    }
//                }
//                else {
//                    // We've got some sub-tests to examine.  First get the boolean operation used

//                    int TrueCount  = 0;
//                    int FalseCount = 0;

//                    string BooleanMethod = ((XmlElement)Context).GetAttribute("boolean");
			
//                    if ((Context.Name != "test") || (BooleanMethod == ""))
//                        BooleanMethod = "and";

//                    // Find and run all sub-tests
//                    XmlNodeList TestNodeSet = Context.SelectNodes("test");

//                    foreach (XmlElement TestNode in TestNodeSet) {
//                        bool NextCheck = true;

//                        // Get the next test's result
//                        NextCheck = CheckTests(TestNode, Application, SoapNamespaceManager);

//                        if (NextCheck)
//                            TrueCount++;
//                        else
//                            FalseCount++;

//                        // If at any time we know the response, don't bother figuring anything else out

//                        if      ((BooleanMethod == "and")  && (!NextCheck))
//                            // False and (*) == False
//                            return false;
//                        else if ((BooleanMethod == "nand") && (!NextCheck))
//                            // False nand (*) == True
//                            return true;
//                        else if ((BooleanMethod == "not")  &&  (NextCheck))
//                            // not True == False
//                            return false;
//                        else if ((BooleanMethod == "or")   &&  (NextCheck))
//                            // True or (*) == True
//                            return true;
//                        else if ((BooleanMethod == "nor")  &&  (NextCheck))
//                            // True nor (*) == False
//                            return false;
//                        else if ((BooleanMethod == "xor")  &&  (NextCheck))
//                            // Count(True) > 1 == False
//                            if (TrueCount > 1)
//                                return false;
//                    }

//                    // We ran through all the tests, return the opposite of above

//                    if      (BooleanMethod == "and")
//                        return true;
//                    else if (BooleanMethod == "nand")
//                        return false;
//                    else if (BooleanMethod == "not")
//                        return true;
//                    else if (BooleanMethod == "or")
//                        return false;
//                    else if (BooleanMethod == "nor")
//                        return true;
//                    else if (BooleanMethod == "xor")
//                        return (TrueCount == 1);
//                }

//                // No test was actually found, just return true

//                return true;
//            }


//            /// <summary>
//            /// Set permissions on a message queue.
//            /// </summary>
//            /// <param name="mqQueue">The message queue.</param>
//            /// <param name="AccessRights">The access rights (read, write, etc.) to set.</param>
//            /// <param name="EntryType">The entry type (allow, deny, etc.)</param>
//            protected void SetQueuePermissions(MessageQueue mqQueue, MessageQueueAccessRights AccessRights, AccessControlEntryType EntryType) {
//                // Get the list of users allowed to access this queue
//                string[] QueueUserGroupNames = new string[0];
//                string Description           = null;
//                try {
//                    Microsoft.Win32.RegistryKey GroupKeys = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(_QueueUserGroupsKey);
//                    QueueUserGroupNames = ((string)GroupKeys.GetValue("UserGroups")).Split(',');
//                    Description         = (string)GroupKeys.GetValue("UserGroupDesc");
//                }
//                catch {
//                }
//                lock (m_oLockObject) {
//                    // Add each user to the ACL for this queue with full permission
//                    foreach(string GroupNamePair in QueueUserGroupNames) {
//                        string GroupName = GroupNamePair.Trim();
//                        try {
//                            if (mqQueue.MachineName == ".") {
//                                //local machine, don't bother prepending anything to the group name
//                                try {
//                                    mqQueue.SetPermissions(new MessageQueueAccessControlEntry(new Trustee(GroupName, null, TrusteeType.Group), AccessRights, EntryType));
//                                }
//                                catch (InvalidOperationException) {
//                                    if (GroupName.IndexOf("\\") < 0) {
//                                        //try to create the local group
//                                        CreateGroup(null, GroupName, Description);
//                                        //try and set the permissions again
//                                        mqQueue.SetPermissions(new MessageQueueAccessControlEntry(new Trustee(GroupName, null, TrusteeType.Group), AccessRights, EntryType));
//                                    }
//                                }
//                            }
//                            else if (GroupName.IndexOf("\\") > 0) {
//                                //domain user. don't bother setting permissions for machine users on remote machines, it doesn't work.
//                                mqQueue.SetPermissions(new MessageQueueAccessControlEntry(new Trustee(GroupName, null, TrusteeType.Group), AccessRights, EntryType));
//                            }
//                            /*else {
//                                //remote machine, prepend the machine name to the group name, if the group name is not a domain user
//                                try {
//                                    if (GroupName.IndexOf("\\") < 0) {
//                                        //no slash, machine user
//                                        mqQueue.SetPermissions(new Messaging.MessageQueueAccessControlEntry(new Messaging.Trustee(mqQueue.MachineName + "\\" + GroupName, null, Messaging.TrusteeType.Group), AccessRights, EntryType));
//                                    }
//                                    else {
//                                        //domain user
//                                        mqQueue.SetPermissions(new Messaging.MessageQueueAccessControlEntry(new Messaging.Trustee(GroupName, null, Messaging.TrusteeType.Group), AccessRights, EntryType));
//                                    }
//                                }
//                                catch (InvalidOperationException) {
//                                    if (GroupName.IndexOf("\\") < 0) {
//                                        //try to create the machine-local group
//                                        CreateGroup(mqQueue.MachineName, GroupName, Description);
//                                        //try and set the permissions again
//                                        mqQueue.SetPermissions(new Messaging.MessageQueueAccessControlEntry(new Messaging.Trustee(mqQueue.MachineName + "\\" + GroupName, null, Messaging.TrusteeType.Group), AccessRights, EntryType));
//                                    }
//                                }
//                            }*/
//                        }
//                        catch (Exception ex) {
//                            MessageQueueException mqeError = new MessageQueueException(null, ex.GetType().ToString(), "There was an error setting permissions for the \"" + mqQueue.Path + "\" queue. Since the queue has already been opened, this is a non-fatal error. Execution will continue. " + ex.Message, ex.StackTrace, _ToolName, _ConfigNode, new string[] {});
//                            mqeError.Send(_ErrorQueueName);
//                        }
//                    }
//                }
//            }

//            /// <summary>
//            /// Opens the message queue specified for output creating if necessary.
//            /// Queue opening operations are expensive so if you want a queue, use 
//            /// <c>FindQueue()</c> which caches queue connections.
//            /// </summary>
//            /// <param name="sQueuePath">Path of the Queue to open</param>
//            /// <returns>MessageQueue designated the path specified</returns>
//            protected MessageQueue OpenQueue(string sQueuePath) {
//                // Lock the queue list so we don't jack anything up with concurrency issues
//                lock (this) {
//                    MessageQueue mqQueue = null;
//                    try {
//                        if (sQueuePath.ToLower().StartsWith("formatname:direct=os:")) {
//                            //Create will throw an exception if the queue already exists
//                            mqQueue = MessageQueue.Create(sQueuePath.Substring(21));
//                        }
//                        else {
//                            //Create will throw an exception if the queue already exists
//                            mqQueue = MessageQueue.Create(sQueuePath);
//                        }
//                        SetQueuePermissions(mqQueue, MessageQueueAccessRights.FullControl, AccessControlEntryType.Allow);
//                        mqQueue.Close();
//                        return new MessageQueue(sQueuePath);
//                    }
//                    catch (System.Messaging.MessageQueueException mqe) {
//                        if (mqe.MessageQueueErrorCode == MessageQueueErrorCode.QueueExists) {
//                            mqQueue = new MessageQueue(sQueuePath);
//                            return mqQueue;
//                        }
//                        else if (mqe.MessageQueueErrorCode == MessageQueueErrorCode.IllegalQueuePathName) {
//                            return new MessageQueue(sQueuePath);
//                        }
//                        else {
//                            throw mqe;
//                        }
//                    }
//                }
//            }

//            /// <summary>
//            /// Creates a group on the given domain with a particular description
//            /// </summary>
//            /// <param name="Domain"></param>
//            /// <param name="Name"></param>
//            /// <param name="Description"></param>
//            private void CreateGroup(string Domain, string Name, string Description) {
//                if ((Domain == null) || (Domain == ""))
//                    Domain = System.Net.Dns.GetHostName();

//                if (Description == null)
//                    Description = "Users belonging to this group are given full control over any message queues created by the Aggregation System";

//                try {
//                    DirectoryServices.DirectoryEntry LocalMachine = new DirectoryServices.DirectoryEntry("WinNT://" + Domain);
//                    DirectoryServices.DirectoryEntries LocalMachineEntries = LocalMachine.Children;
//                    DirectoryServices.DirectoryEntry NewGroup = LocalMachineEntries.Add(Name, "Group");
//                    NewGroup.Properties["Description"].Add(Description);
//                    NewGroup.CommitChanges();
//                }
//                catch (Exception exx) {
//                    exx.GetType();
//                }
//            }

//            /// <summary>
//            /// Finds a queue within the OutputQueue ArrayList.
//            /// </summary>
//            /// <param name="QueuePath">Path of the Queue to Find</param>
//            /// <returns>The MessageQueue itself</returns>
//            protected MessageQueue FindQueue(string QueuePath) {
//                MessageQueue CorrectQueue = null;
//                // We don't want other threads adding queues while we're looking for ours
//                lock (_OutputQueues) {
//                    try {
//                        // Check the path of each queue to find ours
//                        // Might be nice to add a bubbling mechanism here so more common queues are
//                        // found sooner
//                        foreach (MessageQueue QueueToCheck in _OutputQueues) {
//                            if (QueueToCheck.Path == QueuePath) {
//                                CorrectQueue = QueueToCheck;
//                                break;
//                            }
//                        }
//                        // If we've found our queue, we're done.  Otherwise we have to create one
//                        // and throw it in the ArrayList of queues
//                        if (CorrectQueue == null) {
//                            try {
//                                CorrectQueue = OpenQueue(QueuePath);
//                                _OutputQueues.Add(CorrectQueue);
//                            }
//                            catch (Exception exx) {
//                                // Report Error: Queue Could Not Be Opened
//                                ReportError(null, exx);
//                            }
//                        }
//                    }
//                    catch (MessageQueueException mexx) {
//                        throw mexx;
//                    }
//                    catch (Exception exx) {
//                        // There was some weird error.  Not sure what, but reporting just in case
//                        ReportError(null, exx);
//                    }
//                }
//                return CorrectQueue;
//            }

//            /// <summary>
//            /// Immediately reports an error to the error queue
//            /// </summary>
//            /// <param name="Application">The user's application.  If unavailable, use null</param>
//            /// <param name="ex">The exception which was thrown</param>
//            protected virtual void ReportErrorNow(XmlDocument Application, Exception ex) {
//                System.Diagnostics.Debug.WriteLine(ex.ToString());
//                ReportErrorNow(Application, ex.GetType().ToString(), ex.Message, ex.StackTrace, ex.ToString());
//            }

//            /// <summary>
//            /// Immediately reports an error to the error queue
//            /// </summary>
//            /// <param name="Application">The user's application.  If unavailable, use null</param>
//            /// <param name="ex">The exception which was thrown</param>
//            protected virtual void ReportErrorNow(XmlDocument Application, string Message, Exception ex) {
//                ReportErrorNow(Application, ex.GetType().ToString(), Message, ex.StackTrace, ex.Message, ex.ToString());
//            }


//            /// <summary>
//            /// Immediately reports an error to the error queue
//            /// </summary>
//            /// <param name="Application">The user's application.  If unavailable, use null</param>
//            /// <param name="Message">The message to report</param>
//            protected virtual void ReportErrorNow(XmlDocument Application, string Message) {
//                ReportErrorNow(Application, "", Message, "");
//            }


//            /// <summary>
//            /// Immediately reports an error to the error queue
//            /// </summary>
//            /// <param name="Application">The user's application.  If unavailable, use null</param>
//            /// <param name="Label">A short label for the message</param>
//            /// <param name="Message">The message to report</param>
//            protected virtual void ReportErrorNow(XmlDocument Application, string Label, string Message) {
//                ReportErrorNow(Application, Label, Message, "");
//            }


//            /// <summary>
//            /// Immediately reports an error to the error queue
//            /// </summary>
//            /// <param name="Application">The user's application.  If unavailable, use null</param>
//            /// <param name="Label">A short label for the message</param>
//            /// <param name="Message">The message to report</param>
//            /// <param name="Trace">The stack trace</param>
//            /// <param name="MoreInfo">Array of addition info which may be useful</param>
//            protected virtual void ReportErrorNow(XmlDocument Application, string Label, string Message, string Trace, params string[] MoreInfo) {
//                try {
//                    ReportError(Application, Label, Message, Trace, MoreInfo);
//                }
//                catch (MessageQueueException exx) {
//                    exx.Send(_ErrorQueueName);
//                    exx.Log();
//                    MonitorSend(MessageType.Error, "", _ErrorQueueName, null);
//                }
//            }

//            /// <summary>
//            /// Throws a MessageQueueException which can be caught and "sent" to the
//            /// error queue (done automatically if thrown from <c>ProcessApplication()</c>
//            /// </summary>
//            /// <param name="Application">The user's application.  If unavailable, use null</param>
//            /// <param name="ex">The exception which was thrown</param>
//            protected virtual void ReportError(XmlDocument Application, Exception ex) {
//                if (ex is MessageQueueException) {
//                    throw ex;
//                }
//                else if (!(ex is Threading.ThreadInterruptedException)) {
//                    ReportError(Application, ex.GetType().ToString(), ex.Message, ex.StackTrace, ex.ToString());
//                }
//            }

//            /// <summary>
//            /// Throws a MessageQueueException which can be caught and "sent" to the
//            /// error queue (done automatically if thrown from <c>ProcessApplication()</c>
//            /// </summary>
//            /// <param name="Application">The user's application.  If unavailable, use null</param>
//            /// <param name="ex">The exception which was thrown</param>
//            protected virtual void ReportError(XmlDocument Application, Exception ex, params string[] Info) {
//                if (!(ex is Threading.ThreadInterruptedException))
//                    ReportError(Application, ex.GetType().ToString(), ex.Message, ex.StackTrace, Info);
//            }


//            /// <summary>
//            /// Throws a MessageQueueException which can be caught and "sent" to the
//            /// error queue (done automatically if thrown from <c>ProcessApplication()</c>
//            /// </summary>
//            /// <param name="Application">The user's application.  If unavailable, use null</param>
//            /// <param name="Message">The message to report</param>
//            protected virtual void ReportError(XmlDocument Application, string Message) {
//                ReportError(Application, "", Message, "");
//            }


//            /// <summary>
//            /// Throws a MessageQueueException which can be caught and "sent" to the
//            /// error queue (done automatically if thrown from <c>ProcessApplication()</c>
//            /// </summary>
//            /// <param name="Application">The user's application.  If unavailable, use null</param>
//            /// <param name="Label">A short label for the message</param>
//            /// <param name="Message">The message to report</param>
//            protected virtual void ReportError(XmlDocument Application, string Label, string Message) {
//                ReportError(Application, Label, Message, "");
//            }


//            /// <summary>
//            /// Throws a MessageQueueException which can be caught and "sent" to the
//            /// error queue (done automatically if thrown from <c>ProcessApplication()</c>
//            /// </summary>
//            /// <param name="Application">The user's application.  If unavailable, use null</param>
//            /// <param name="Label">A short label for the message</param>
//            /// <param name="Message">The message to report</param>
//            /// <param name="Trace">The stack trace</param>
//            /// <param name="MoreInfo">Array of addition info which may be useful</param>
//            protected virtual void ReportError(XmlDocument Application, string Label, string Message, string Trace, params string[] MoreInfo) {
//                if (Application != null) {
//                    System.Diagnostics.EventLog.WriteEntry("Aggregator", Label + "\n\n" + Message + "\n\n" + Trace, System.Diagnostics.EventLogEntryType.Error, 0, 0, System.Text.ASCIIEncoding.ASCII.GetBytes(Application.OuterXml));
//                }
//                else {
//                    System.Diagnostics.EventLog.WriteEntry("Aggregator", Label + "\n\n" + Message + "\n\n" + Trace, System.Diagnostics.EventLogEntryType.Error, 0, 0, null);
//                }
//                throw new MessageQueueException(Application, Label, Message, Trace, _ToolName, _ConfigNode, MoreInfo);
//            }

//            //			private bool SetupCategory()
//            //			{        
//            //				//				if (System.Diagnostics.PerformanceCounterCategory.Exists("Aggregator") ) 
//            //				//					System.Diagnostics.PerformanceCounterCategory.Delete("Aggregator");
//            //
//            //				
//            //				if ( !System.Diagnostics.PerformanceCounterCategory.Exists("Aggregator") ) 
//            //				{
//            //					System.Diagnostics.CounterCreationDataCollection CCDC = new System.Diagnostics.CounterCreationDataCollection();
//            //            
//            //					// Add the counter.
//            //					System.Diagnostics.CounterCreationData MessageRead = new System.Diagnostics.CounterCreationData();
//            //					MessageRead.CounterType = System.Diagnostics.PerformanceCounterType.RateOfCountsPerSecond32;
//            //					MessageRead.CounterName = "Messages Read";
//            //					CCDC.Add(MessageRead);
//            //            
//            //					System.Diagnostics.CounterCreationData MessageWritten = new System.Diagnostics.CounterCreationData();
//            //					MessageWritten.CounterType = System.Diagnostics.PerformanceCounterType.RateOfCountsPerSecond32;
//            //					MessageWritten.CounterName = "Messages Written";
//            //					CCDC.Add(MessageWritten);
//            //            
//            //					System.Diagnostics.CounterCreationData MessageTime = new System.Diagnostics.CounterCreationData();
//            //					MessageTime.CounterType = System.Diagnostics.PerformanceCounterType.AverageCount64;
//            //					MessageTime.CounterName = "Processing Time";
//            //					CCDC.Add(MessageTime);
//            //            
//            //					System.Diagnostics.CounterCreationData MessageTimeBase = new System.Diagnostics.CounterCreationData();
//            //					MessageTimeBase.CounterType = System.Diagnostics.PerformanceCounterType.AverageBase;
//            //					MessageTimeBase.CounterName = "Processing Time Base";
//            //					CCDC.Add(MessageTimeBase);
//            //            
//            //					// Create the category.
//            //					System.Diagnostics.PerformanceCounterCategory.Create("Aggregator", 
//            //						"Aggregator Logging", 
//            //						CCDC);
//            //                
//            //					return(true);
//            //				}
//            //				else
//            //				{
//            //					return(false);
//            //				}
//            //			}

//            //			private void CreateCounters()
//            //			{
//            //				_PerformanceRead              = new System.Diagnostics.PerformanceCounter("Aggregator", "Messages Read",        _ToolName, false);
//            //				_PerformanceReadAll           = new System.Diagnostics.PerformanceCounter("Aggregator", "Messages Read",                   false);
//            //				_PerformanceWrite             = new System.Diagnostics.PerformanceCounter("Aggregator", "Messages Written",     _ToolName, false);
//            //				_PerformanceWriteAll          = new System.Diagnostics.PerformanceCounter("Aggregator", "Messages Written",                false);
//            //				_PerformanceProcessing        = new System.Diagnostics.PerformanceCounter("Aggregator", "Processing Time",      _ToolName, false);
//            //				_PerformanceProcessingBase    = new System.Diagnostics.PerformanceCounter("Aggregator", "Processing Time Base", _ToolName, false);
//            //				_PerformanceProcessingAll     = new System.Diagnostics.PerformanceCounter("Aggregator", "Processing Time",                 false);
//            //				_PerformanceProcessingAllBase = new System.Diagnostics.PerformanceCounter("Aggregator", "Processing Time Base",            false);
//            //			}

    
//            /// <summary>
//            /// Informs the Monitor queue that a message was recieved from a queue
//            /// </summary>
//            /// <param name="IncomingQueueName">The queue the message was read from</param>
//            protected virtual void MonitorRecieve(MessageType Type, string IncomingQueueName, string ID) {
//                if (MonitorMessages) {
//                    //_PerformanceRead.Increment();
//                    //_PerformanceReadAll.Increment();
//                    ReceivedMessage MessageInfo = new ReceivedMessage(Type, DateTime.Now, IncomingQueueName, this._ToolName, ID);
//                    _Incoming++;
//                    if (_MonitorQueueName != "") {
//                        if (_MonitorQueue == null) {
//                            _MonitorQueue = OpenQueue(_MonitorQueueName);
//                            _MonitorQueue.DefaultPropertiesToSend.Recoverable = false;
//                            _MonitorQueue.DefaultPropertiesToSend.UseDeadLetterQueue = false;
//                            _MonitorQueue.DefaultPropertiesToSend.TimeToBeReceived = new TimeSpan(0, 0, 10);
//                        }
//                        lock (_MonitorQueue) {
//                            _MonitorQueue.Send(MessageInfo, "Receive");
//                        }
//                    }
//                }
//            }
		
//            /// <summary>
//            /// Informs the Monitor queue that a message was sent to a queue
//            /// </summary>
//            /// <param name="IncomingQueueName">The queue the message was read from</param>
//            /// <param name="OutgoingQueueName">The queue the message was sent to</param>
//            protected virtual void MonitorSend(MessageType Type, string IncomingQueueName, string OutgoingQueueName, string ID) {
//                if (MonitorMessages) {
//                    //_PerformanceWrite.Increment(); 
//                    //_PerformanceWriteAll.Increment();
//                    SentMessage MessageInfo = new SentMessage(Type, DateTime.Now, OutgoingQueueName, IncomingQueueName, this._ToolName, ID);
//                    _Outgoing++;
//                    if (_MonitorQueueName != "") {
//                        lock (_MonitorQueueMonitor) {
//                            if (_MonitorQueue == null) {
//                                _MonitorQueue = OpenQueue(_MonitorQueueName);
//                                _MonitorQueue.DefaultPropertiesToSend.Recoverable = false;
//                                _MonitorQueue.DefaultPropertiesToSend.UseDeadLetterQueue = false;
//                                _MonitorQueue.DefaultPropertiesToSend.TimeToBeReceived = new TimeSpan(0, 0, 10);
//                            }
//                            _MonitorQueue.Send(MessageInfo, "Send");
//                        }
//                    }
//                }
//            }

		
//            /// <summary>
//            /// Sends a message to the log queue
//            /// </summary>
//            /// <param name="ApplicationID">ID of the application</param>
//            /// <param name="Server">Server the application was read from</param>
//            /// <param name="IncomingQueue">Queue the message was read from</param>
//            /// <param name="Action">Description of the Action that was performed</param>
//            protected virtual void LogAction(string ApplicationID, string Server, string IncomingQueue, string Action, XmlElement SoapMessage) {
//                if (m_bSendLogMessages) {
//                    // Connect to the log queue if necessary
//                    if (_LogQueue == null) {
//                        if (!_LogQueueName.ToLower().StartsWith("formatname") && _LogQueueName.ToLower().IndexOf("private$") > -1) {
//                            _LogQueueName = "FormatName:DIRECT=OS:" + _LogQueueName;
//                        }
//                        _LogQueue = OpenQueue(_LogQueueName);
//                    }

//                    // I really don't like building a DOM, it may prove to be too expensive, but
//                    // it does handle xml character encoding so....

//                    // New Note: This should be changed to an XML Writer

//                    XmlDocument LogData    = new XmlDocument();
//                    XmlElement  LogEnv     = LogData.CreateElement("soap", "Envelope", _SoapURN);
//                    XmlElement  LogBody    = LogData.CreateElement("soap", "Body", _SoapURN);
//                    XmlElement  LogNode    = LogData.CreateElement("log");
//                    XmlElement  LogID      = LogData.CreateElement("id");
//                    XmlElement  LogServer  = LogData.CreateElement("server");
//                    XmlElement  LogQueue   = LogData.CreateElement("queue");
//                    XmlElement  LogActor   = LogData.CreateElement("actor");
//                    XmlElement  LogAction  = LogData.CreateElement("action");
//                    XmlElement  LogMachine = LogData.CreateElement("machine");
//                    XmlElement  LogTime    = LogData.CreateElement("timestamp");
//                    XmlElement  LogApp     = LogData.CreateElement("data");

//                    LogID.AppendChild(LogData.CreateTextNode(ApplicationID));
//                    LogServer.AppendChild(LogData.CreateTextNode(Server));
//                    LogQueue.AppendChild(LogData.CreateTextNode(IncomingQueue));
//                    LogActor.AppendChild(LogData.CreateTextNode(_ToolName));
//                    LogAction.AppendChild(LogData.CreateTextNode(Action));
//                    LogMachine.AppendChild(LogData.CreateTextNode(System.Net.Dns.GetHostName()));
//                    LogTime.AppendChild(LogData.CreateTextNode(DateTime.Now.ToString()));
//                    if (SoapMessage != null) {
//                        LogApp.AppendChild(LogData.ImportNode(SoapMessage, true));
//                    }
//                    LogNode.AppendChild(LogID);
//                    LogNode.AppendChild(LogServer);
//                    LogNode.AppendChild(LogQueue);
//                    LogNode.AppendChild(LogActor);
//                    LogNode.AppendChild(LogAction);
//                    LogNode.AppendChild(LogMachine);
//                    LogNode.AppendChild(LogTime);
//                    LogNode.AppendChild(LogApp);
//                    LogBody.AppendChild(LogNode);
//                    LogEnv.AppendChild(LogBody);
//                    LogData.AppendChild(LogEnv);
//                    lock (_LogQueue) {
//                        _LogQueue.Send(LogData, ApplicationID);
//                    }
//                    MonitorSend(MessageType.Log, "", _LogQueueName, null);
//                }
//            }

//            /// <summary>
//            /// Sends a message to the log queue
//            /// </summary>
//            /// <param name="ApplicationNode">User's application</param>
//            /// <param name="IncomingQueue">Queue the message was read from</param>
//            /// <param name="Action">Description of the Action that was performed</param>
//            protected virtual void LogAction(XmlElement ApplicationNode, string IncomingQueue, string Action) {
//                string ApplicationID = "";
//                string Server = "";
//                if (ApplicationNode != null) {
//                    ApplicationID = ApplicationNode.GetAttribute("id");
//                    Server = ApplicationNode.GetAttribute("server");
//                }
//                LogAction(ApplicationID, Server, IncomingQueue, Action, ApplicationNode);
//            }
			
//            /// <summary>
//            /// Sends debugging messages to a queue where the producer or developer
//            /// can read and decipher the inner workings of the component
//            /// </summary>
//            /// <param name="Message">Debug Message</param>
//            /// <param name="MessageType">
//            ///	Type of Message from DebugMessageType enumeration.  Note that Core
//            ///	and Development messages are not sent when the project is built in 
//            ///	release mode
//            ///	</param>
//            public virtual void Debug(string Label, string Message, DebugMessageType MessageType) {
//                Debug(Label, Message, MessageType, null);
//            }
			
			
//            /// <summary>
//            /// Sends a debugging message to the debug queue if it exists
//            /// </summary>
//            /// <param name="Label">Label for the message</param>
//            /// <param name="Message">Message To Be Sent</param>
//            /// <param name="MessageType">Type of Message (core/component/user)</param>
//            /// <param name="DebugQueue">Queue to be sent to</param>
//            public virtual void Debug(string Label, string Message, DebugMessageType MessageType, string DebugQueue) {
//                if ((DebugQueue == "") || (DebugQueue == null))
//                    DebugQueue = _DebugQueueName;

//                if (DebugQueue != "") {
//                    System.Console.WriteLine(Label + ": " + Message);

//                    string DebugXML =     "<soap:Envelope xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/' soap:id='" + NewGuid + "'>";
//                    DebugXML = DebugXML +   "<soap:Body>";
//                    DebugXML = DebugXML +     "<debug/>";
//                    DebugXML = DebugXML +   "</soap:Body>";
//                    DebugXML = DebugXML + "</soap:Envelope>";

//                    XmlDocument DebugDoc = new XmlDocument();
//                    DebugDoc.LoadXml(DebugXML);
					
//                    XmlElement DebugElement = (XmlElement)DebugDoc.SelectSingleNode("//debug");

//                    XmlElement NewLabel = DebugDoc.CreateElement("label");
//                    NewLabel.AppendChild(DebugDoc.CreateTextNode(Label));
//                    DebugElement.AppendChild(NewLabel);

//                    XmlElement NewType = DebugDoc.CreateElement("type");
//                    switch (MessageType) {
//                        case DebugMessageType.Core:
//                            NewType.AppendChild(DebugDoc.CreateTextNode("Core"));
//                            break;
//                        case DebugMessageType.Development:
//                            NewType.AppendChild(DebugDoc.CreateTextNode("Development"));
//                            break;
//                        case DebugMessageType.Production:
//                            NewType.AppendChild(DebugDoc.CreateTextNode("Production"));
//                            break;
//                        case DebugMessageType.User:
//                            NewType.AppendChild(DebugDoc.CreateTextNode("User"));
//                            break;
//                    }
//                    DebugElement.AppendChild(NewType);
//                    XmlElement NewMessage = DebugDoc.CreateElement("message");
//                    NewMessage.AppendChild(DebugDoc.CreateTextNode(Message));
//                    DebugElement.AppendChild(NewMessage);
//                    XmlElement NewMachine = DebugDoc.CreateElement("machine");
//                    NewMachine.AppendChild(DebugDoc.CreateTextNode(System.Net.Dns.GetHostName()));
//                    DebugElement.AppendChild(NewMachine);
//                    XmlElement NewTimeStamp = DebugDoc.CreateElement("timestamp");
//                    NewTimeStamp.AppendChild(DebugDoc.CreateTextNode(System.DateTime.Now.ToString()));
//                    DebugElement.AppendChild(NewTimeStamp);
//                    if (_DebugQueue == null) {
//                        _DebugQueue = OpenQueue(DebugQueue);
//                    }
//                    lock (_DebugQueue) {
//                        _DebugQueue.Send(DebugDoc, Label + " (" + this._ToolName + ")");
//                    }
//                    MonitorSend(_CoreLibrary.MessageType.Debug, "", DebugQueue, null);
//                }
//            }
		
//            /// <summary>
//            /// Enumerated list of all available debug message types
//            /// </summary>
//            public enum DebugMessageType {
//                Core,
//                Development,
//                Production,
//                User
//            }

//            protected XmlElement Transform(XmlDocument OldDocument, XslTransform CommandXsl) {
//                XmlDocument NewDocument = new XmlDocument();
//                NewDocument.Load(CommandXsl.Transform(OldDocument, (XsltArgumentList)null, (XmlResolver)null));
//                return NewDocument.DocumentElement;
//            }
			
//            protected XmlElement Transform(XmlDocument OldDocument, XmlDocument XSLDocument) {
//                XslTransform CommandXsl  = new XslTransform();
//                CommandXsl.Load(XSLDocument, null, null);
//                return Transform(OldDocument, CommandXsl);
//            }
			
//            protected XmlElement Transform(XmlElement OldChunk, XmlDocument XSLDocument) {
//                XmlDocument OldDocument = new XmlDocument();
//                OldDocument.AppendChild(OldDocument.ImportNode(OldChunk, true));
//                //OldDocument.LoadXml(OldChunk.OuterXml);

//                return Transform(OldDocument, XSLDocument);
//            }


//            protected XmlElement Transform(XmlElement OldChunk, string XSLSource) {
//                XmlDocument	 XSLDocument = new XmlDocument();

//                XSLDocument.LoadXml(XSLSource);	
					
//                return Transform(OldChunk, XSLDocument);
//            }

		
//            protected XmlElement Transform(XmlDocument OldDocument, string XSLSource) {
//                XmlDocument	 XSLDocument = new XmlDocument();

//                XSLDocument.LoadXml(XSLSource);	
					
//                return Transform(OldDocument, XSLDocument);
//            }

		
//            protected XmlElement Transform(XmlDocument OldDocument, XmlElement XSLNode) {
//                XslTransform XSLDocument = new XslTransform();
//                XSLDocument.Load(XSLNode, (XmlResolver)null, (System.Security.Policy.Evidence)null);
//                return Transform(OldDocument, XSLDocument);
//            }
		
//            protected XmlElement Transform(XmlElement OldChunk, XmlElement XSLNode) {
//                XmlDocument	 XSLDocument = new XmlDocument();
//                XSLDocument.LoadXml(XSLNode.OuterXml);	
//                return Transform(OldChunk, XSLDocument);
//            }

//            private class Timer {
//                public static Collections.ArrayList getActions(Timer[] AllActions) {
//                    Collections.ArrayList RetVal = new Collections.ArrayList();
//                    foreach (Timer Current in AllActions) {
//                        if (Current.Expired()) {
//                            RetVal.Add(Current);
//                        }
//                    }
//                    return (RetVal);
//                }
//                public Timer() {
//                    m_Interval = 1;
//                    m_LastTime = 0;
//                }
//                public int Interval {
//                    get {
//                        return (m_Interval);
//                    }
//                    set {
//                        m_Interval = value;
//                        if (m_Interval != 0) m_LastTime = (System.Environment.TickCount) % m_Interval;
//                        else m_LastTime = 0;
//                    }
//                }
//                public XmlElement MessageParent {
//                    get {
//                        return (m_Message);
//                    }
//                    set {
//                        m_Message = value;
//                    }
//                }
//                private bool Expired() {
//                    bool RetVal = (System.Environment.TickCount % this.m_Interval) < this.m_LastTime;
//                    this.m_LastTime = System.Environment.TickCount % this.m_Interval;
//                    return RetVal;
//                }
//                private int m_Interval;
//                private int m_LastTime;
//                private XmlElement m_Message;
//            }


//            protected sealed class DateTimeRanges {
//                public struct DateTimeRange {
//                    public DateTime StartTime;
//                    public DateTime FinishTime;


//                    public DateTimeRange(DateTime Start, DateTime Finish) {
//                        StartTime = Start;
//                        FinishTime = Finish;
//                    }
					
//                    public bool InRange(TimeSpan Time, DayOfWeek Day) {
//                        DateTime TestDate = new DateTime(Time.Ticks);
//                        TestDate.AddDays((int)Day);
						
//                        if ((TestDate > StartTime) && (TestDate < FinishTime)) return true;

//                        TestDate.AddDays(7);

//                        if ((TestDate > StartTime) && (TestDate < FinishTime)) return true;

//                        return false;
//                    }
//                }

//                private Collections.ArrayList Ranges = new Collections.ArrayList();
				

//                private void Add(TimeSpan StartTime, int StartDay, TimeSpan FinishTime, int FinishDay) {
//                    DateTime StartDate = new DateTime(StartTime.Ticks);
//                    StartDate.AddDays(StartDay);
					
//                    DateTime FinishDate = new DateTime(FinishTime.Ticks);
//                    FinishDate.AddDays(FinishDay);
					
//                    if (FinishDate < StartDate) FinishDate.AddDays(7);

//                    Ranges.Add(new DateTimeRange(StartDate, FinishDate));
//                }

//                public void Add(TimeSpan StartTime, DayOfWeek StartDay, TimeSpan FinishTime, DayOfWeek FinishDay, bool Continuous) {
//                    if (Continuous)
//                        Add(StartTime, (int)StartDay, FinishTime, (int)FinishDay);
//                    else {
//                        int Start = (int)StartDay;
//                        int Finish = (int)FinishDay;

//                        if (Finish < Start) Finish += 7;

//                        for (int I = Start; I < Finish; I++)
//                            Add(StartTime, I, FinishTime, I);
//                    }
//                }

//                public void Clear() {
//                    Ranges = new Collections.ArrayList();
//                }

//                public bool Current {
//                    get {
//                        foreach (DateTimeRange Range in Ranges) {
//                            if (Range.InRange(DateTime.Now.TimeOfDay, DateTime.Now.DayOfWeek))
//                                return true;
//                        }
					
//                        return false;
//                    }
//                }

//                public void Load(XmlNodeList Nodes, TimeSpan Offset) {
//                    foreach (XmlElement Node in Nodes) {
//                        TimeSpan StartTime = new TimeSpan(0, 0, 0, 0, 0);
//                        TimeSpan FinishTime = new TimeSpan(0, 23, 59, 59, 0);
//                        try {
//                            if (Node.GetAttribute("starttime") != "") {
//                                StartTime = TimeSpan.Parse(Node.GetAttribute("starttime"));
//                            }
//                            if (Node.GetAttribute("finishtime") != "") {
//                                FinishTime = TimeSpan.Parse(Node.GetAttribute("finishtime"));
//                            }
//                        }
//                        catch {
//                            StartTime = new TimeSpan(0, 0, 0, 0, 0);
//                            FinishTime = new TimeSpan(0, 23, 59, 59, 0);
//                        }
//                        StartTime  = StartTime.Add(Offset);
//                        FinishTime = FinishTime.Add(Offset);
//                        DayOfWeek StartDay = DayOfWeek.Monday;
//                        DayOfWeek FinishDay = DayOfWeek.Monday;
//                        bool FoundStart  = true;
//                        bool FoundFinish = true;
//                        switch (Node.GetAttribute("startday").ToLower()) {
//                            case "sunday":
//                                StartDay = DayOfWeek.Sunday;
//                                break;
//                            case "monday":
//                                StartDay = DayOfWeek.Monday;
//                                break;
//                            case "tuesday":
//                                StartDay = DayOfWeek.Tuesday;
//                                break;
//                            case "wednesday":
//                                StartDay = DayOfWeek.Wednesday;
//                                break;
//                            case "thursday":
//                                StartDay = DayOfWeek.Thursday;
//                                break;
//                            case "friday":
//                                StartDay = DayOfWeek.Friday;
//                                break;
//                            case "saturday":
//                                StartDay = DayOfWeek.Saturday;
//                                break;
//                            default:
//                                FoundStart = false;
//                                break;
//                        }
//                        switch (Node.GetAttribute("finishday").ToLower()) {
//                            case "sunday":
//                                FinishDay = DayOfWeek.Sunday;
//                                break;
//                            case "monday":
//                                FinishDay = DayOfWeek.Monday;
//                                break;
//                            case "tuesday":
//                                FinishDay = DayOfWeek.Tuesday;
//                                break;
//                            case "wednesday":
//                                FinishDay = DayOfWeek.Wednesday;
//                                break;
//                            case "thursday":
//                                FinishDay = DayOfWeek.Thursday;
//                                break;
//                            case "friday":
//                                FinishDay = DayOfWeek.Friday;
//                                break;
//                            case "saturday":
//                                FinishDay = DayOfWeek.Saturday;
//                                break;
//                            default:
//                                FoundFinish = false;
//                                break;
//                        }
//                        if (!FoundFinish) {
//                            FinishDay = StartDay;
//                        }
//                        if (!FoundStart) {
//                            StartDay = FinishDay;
//                        }
//                        bool Continuous = true;
//                        if (FoundStart || FoundFinish) {
//                            Add(StartTime, StartDay, FinishTime, FinishDay, Continuous);
//                        }
//                        else {
//                            Add(StartTime, DayOfWeek.Sunday, FinishTime, DayOfWeek.Saturday, false);
//                        }
//                    }
//                }
//            }

//            /// <summary>
//            /// Get or set the flag that says whether monitor messages are enabled.
//            /// </summary>
//            protected bool MonitorMessages {
//                get {
//                    return m_bMonitor;
//                }
//                set {
//                    m_bMonitor = value;
//                }
//            }

//        }
//    }
//}