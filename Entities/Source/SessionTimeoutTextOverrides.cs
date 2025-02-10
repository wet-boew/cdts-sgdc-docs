using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoC.WebTemplate.Entities.Source
{
    public class SessionTimeoutTextOverrides
    {
        /// <summary>
        /// Text for the Continue Session button.
        /// </summary>
        [JsonProperty(PropertyName = "buttonContinue")]
        public string ButtonContinue { get; set; }

        /// <summary>
        /// Text for the End Session button.
        /// </summary>
        [JsonProperty(PropertyName = "buttonEnd")]
        public string ButtonEnd { get; set; }

        /// <summary>
        /// Text for the Sign In button
        /// </summary>
        [JsonProperty(PropertyName = "buttonSignin")]
        public string ButtonSignIn { get; set; }

        /// <summary>
        /// Text for the message displayed below the timer.
        /// </summary>
        [JsonProperty(PropertyName = "timeoutEnd")]
        public string TimeoutEnd { get; set; }

        /// <summary>
        /// Text for the message displayed when the session has expired.
        /// </summary>
        [JsonProperty(PropertyName = "timeoutAlready")]
        public string TimeoutAlready { get; set; }
    }
}
