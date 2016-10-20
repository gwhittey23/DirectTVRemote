using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.SettingsService;
using Windows.UI.Xaml;
using RestSharp.Portable;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DirectTVRemote.Models {

    public class DirectTvAPI {
        public RestCommunicationService m_client;
        public string clientAddr { get; set; } = "0";//No idea if this ever changes as I only have 1 box.
        public DirectTvAPI(string stb_ip) {

            this.m_client = new RestCommunicationService(stb_ip);
        }


        public async Task<Get_tuned_response> GetTuned() {

            DTVRest dtv = new DTVRest();
            string results = await dtv.GetDataAsync(this.m_client, "/tv/getTuned");
            Get_tuned_response m = JsonConvert.DeserializeObject<Get_tuned_response>(results);
            return m;
        }

        public async Task<Get_channel_response> GetChannel(string channel) {

            DTVRest dtv = new DTVRest();
            KeyValuePair<string, string> x = parse_channel(channel);
            string results = await dtv.GetDataAsync(this.m_client, "/tv/getProgInfo?major=" + x.Key + "&minor=" + x.Value + "&clientAddr=" + clientAddr);
            Get_channel_response m = JsonConvert.DeserializeObject<Get_channel_response>(results);
            return m;
            
        }

        public async Task<Get_channel_response> TuneToChannel(string channel) {

            DTVRest dtv = new DTVRest();
            KeyValuePair<string, string> x = parse_channel(channel);
            string results = await dtv.GetDataAsync(this.m_client, "/tv/tune?major=" + x.Key + "&minor=" + x.Value + "&clientAddr=" + clientAddr);
            Get_channel_response m = JsonConvert.DeserializeObject<Get_channel_response>(results);
            return m;

        }

        public async Task<Get_channel_response> KeyPress(string key) {
            string[] valid_keys = { "power", "poweron", "poweroff", "format", "pause", "rew", "replay", "stop",
                          "advance", "ffwd", "record", "play", "guide", "active", "list", "exit",
                          "back", "menu", "info", "up", "down", "left", "right", "select", "red",
                          "green", "yellow", "blue", "chanup", "chandown", "prev", "0", "1", "2",
                          "3", "4", "5", "6", "7", "8", "9", "dash", "enter" };
            if (valid_keys.Contains(key)) {
                DTVRest dtv = new DTVRest();
                string results = await dtv.GetDataAsync(this.m_client, "/remote/processKey?key = " + key + " & clientAddr = " + clientAddr);
                Get_channel_response m = JsonConvert.DeserializeObject<Get_channel_response>(results);
                return m;
            }
            else {
                var response = new Key_press_response();
                response.key_err = true;
                return null;


            }
        }

        public KeyValuePair<string, string> parse_channel(string channel) {
            try {
                string[] strArr = null;
                strArr = channel.Split('-');

                string major = strArr[0];
                string minor = strArr[1];
                return new KeyValuePair<string, string>(major, minor);

            }
            catch (IndexOutOfRangeException e) {
                string major = channel;
                string minor = "65535";
                return new KeyValuePair<string, string>(major, minor);

            }

        }
        
        public string combine_channel(string major, string minor) {
            if (minor == "65535") {
                return major.ToString();

            }
            else {
                return major.ToString() + "-" + minor.ToString();
            }
        }
    }
    public class DTVRest {

        public async Task<string> GetDataAsync(RestCommunicationService t_client, string request_type) {
            var response = await t_client.SendRequestAsync(new RestRequest(request_type));
            return response.Content;
        }
    }



    public class Status {
        public int code { get; set; }
        public int commandResult { get; set; }
        public string msg { get; set; }
        public string query { get; set; }
    }

    public class Get_tuned_response {
        public string callsign { get; set; }
        public string date { get; set; }
        public int duration { get; set; }
        public bool isOffAir { get; set; }
        public int isPclocked { get; set; }
        public bool isPpv { get; set; }
        public bool isRecording { get; set; }
        public bool isVod { get; set; }
        public int major { get; set; }
        public int minor { get; set; }
        public int offset { get; set; }
        public string programId { get; set; }
        public string rating { get; set; }
        public int startTime { get; set; }
        public int stationId { get; set; }
        public Status status { get; set; }
        public string title { get; set; }
    }

    public class Tune__channel_response {
        public Status status { get; set; }
    }

    public class Key_press_response {
        public bool key_err { get; set; }
        public string hold { get; set; }
        public string key { get; set; }
        public Status status { get; set; }
    }

    public class Get_channel_response {
        public string callsign { get; set; }
        public string date { get; set; }
        public int duration { get; set; }
        public string episodeTitle { get; set; }
        public bool isOffAir { get; set; }
        public int isPclocked { get; set; }
        public bool isPpv { get; set; }
        public bool isRecording { get; set; }
        public bool isVod { get; set; }
        public int major { get; set; }
        public int minor { get; set; }
        public string programId { get; set; }
        public string rating { get; set; }
        public int startTime { get; set; }
        public int stationId { get; set; }
        public Status status { get; set; }
        public string title { get; set; }
    }
}
