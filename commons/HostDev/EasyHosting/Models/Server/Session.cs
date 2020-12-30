using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Models.Server {
    public class Session {
        private Dictionary<string, object> SessionData = new Dictionary<string, object>();

        public Session() {

        }

        public void Set(string name, object value) {
            if (SessionData.ContainsKey(name)) {
                SessionData[name] = value;
            }
            else {
                SessionData.Add(name, value);
            }
        }

        public object Get(string name) {
            if (SessionData.ContainsKey(name)) {
                return SessionData[name];
            }
            return null;
        }

        public bool Has(string name) {
            return SessionData.ContainsKey(name);
        }

        public bool Remove(string name) {
            if (SessionData.ContainsKey(name)) {
                SessionData.Remove(name);
                return true;
            }
            return false;
        }
    }
}
