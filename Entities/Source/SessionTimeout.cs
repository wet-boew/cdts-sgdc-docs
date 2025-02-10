using GoC.WebTemplate.Entities.Source;

using Newtonsoft.Json;

namespace GoC.WebTemplate.Components.Entities
{
    public class SessionTimeout
    {
        public SessionTimeout() { }

        public bool Enabled { get; set; }

        /// <summary>
        /// inactivity period of time after which the modal dialog will appear (default 20 minutes).
        /// </summary>
        /// <remarks>time provided in milliseconds</remarks>
        public int Inactivity { get; set; }

        /// <summary>
        /// period of time the user has to perform an action once the modal dialog is displayed (default 3 minutes).
        /// </summary>
        /// <remarks>time provided in milliseconds</remarks>
        public int ReactionTime { get; set; }

        /// <summary>
        /// period of time for the session to stay alive until the modal dialog appears (default 20 minutes).
        /// </summary>
        /// <remarks>time provided in milliseconds</remarks>
        [JsonProperty(PropertyName = "sessionalive")]
        public int SessionAlive { get; set; }

        /// <summary>
        /// URL that users are sent to when the session has expired.
        /// </summary>
        [JsonProperty(PropertyName = "logouturl")]
        public string LogoutUrl { get; set; }

        /// <summary>
        /// URL used to perform an ajax request to determine the validity of the session.
        /// </summary>
        [JsonProperty(PropertyName = "refreshCallbackUrl")]
        public string RefreshCallBackUrl { get; set; }

        /// <summary>
        /// Determines if clicking on the document should reset the inactivity timeout and perform an ajax request (if a refreshCallBackUrl has been specified).
        /// </summary>
        [JsonProperty(DefaultValueHandling=DefaultValueHandling.Include)] 
        public bool RefreshOnClick { get; set; }

        /// <summary>
        /// Sets the amount of time that must pass before an ajax request can be made.
        /// </summary>
        public int RefreshLimit { get; set; }

        /// <summary>
        /// Sets the request method used for ajax requests. Recommended: GET or POST.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Additional data to send with the request.
        /// </summary>
        public string AdditionalData { get; set; }

        /// <summary>
        /// URL used for the Sign In action once the session has expired.
        /// </summary>
        [JsonProperty(PropertyName = "signInUrl")]
        public string SignInUrl { get; set; }

        /// <summary>
        /// Optional button text and message elements to override the defaults.
        /// </summary>
        [JsonProperty(PropertyName = "textOverrides")]
        public SessionTimeoutTextOverrides TextOverrides { get; set; }
    }
}
