using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.Text;
using System.Web;

namespace Loregroup.Core.Helpers
{
    public class WebHttpJsonElement : BehaviorExtensionElement {
        public class WebHttpJsonBehavior : WebHttpBehavior {
            internal class MessageFormatter : IDispatchMessageFormatter {
                Newtonsoft.Json.JsonSerializer serializer = null;
                internal MessageFormatter() {
                    serializer = new Newtonsoft.Json.JsonSerializer();
                }

                public void DeserializeRequest(Message message, object[] parameters) {
                    throw new NotImplementedException();
                }

                public Message SerializeReply(MessageVersion messageVersion, object[] parameters, object result) {
                    var stream = new MemoryStream();
                    var streamWriter = new StreamWriter(stream, Encoding.UTF8);
                    var jtw = new Newtonsoft.Json.JsonTextWriter(streamWriter);
                    serializer.Serialize(jtw, result);
                    jtw.Flush();
                    stream.Seek(0, SeekOrigin.Begin);
                    return WebOperationContext.Current.CreateStreamResponse(stream, "application/json");
                }
            }

            protected override IDispatchMessageFormatter GetReplyDispatchFormatter(OperationDescription operationDescription, ServiceEndpoint endpoint) {
                return new MessageFormatter();
            }
        }

        public WebHttpJsonElement() { }

        public override Type BehaviorType {
            get {
                return typeof(WebHttpJsonBehavior);
            }
        }

        protected override object CreateBehavior() {
            var behavior = new WebHttpJsonBehavior();
            behavior.DefaultBodyStyle = WebMessageBodyStyle.WrappedRequest;
            behavior.DefaultOutgoingResponseFormat = WebMessageFormat.Json;
            behavior.AutomaticFormatSelectionEnabled = false;
            return behavior;
        }
    }
}