using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Models.Server {
    public class Session {
        private Dictionary<string, object> SessionData = new Dictionary<string, object>();

        public Session() {

        }

        /// <summary>
        /// Ustawia wartość w sesji
        /// </summary>
        /// <param name="name">Nazwa wartości do ustawienia</param>
        /// <param name="value">Wartość do ustawienia</param>
        public void Set(string name, object value) {
            if (SessionData.ContainsKey(name)) {
                SessionData[name] = value;
            }
            else {
                SessionData.Add(name, value);
            }
        }

        /// <summary>
        /// Pobiera wartość z sesji
        /// </summary>
        /// <param name="name">Nazwa wartości do pobrania</param>
        /// <returns>Wartość przypisana do podanej nazwy lub NULL jeśli brak przypisanej wartości</returns>
        public object Get(string name) {
            if (SessionData.ContainsKey(name)) {
                return SessionData[name];
            }
            return null;
        }

        /// <summary>
        /// Pobiera wartość sesji i castuje ją na podany typ (jeśli jest to możliwe)
        /// </summary>
        /// <typeparam name="T">Typ na który obiekt w sesji powinien zostać scastowany</typeparam>
        /// <param name="name">Nazwa do wyszukania</param>
        /// <returns>Wartość przypisana do podanej nazwy</returns>
        /// <exception cref="KeyNotFoundException">Wyjątek wyrzucany jeśli podana nazwa nie jest zapisana w sesji</exception>
        public T Get<T>(string name) {
            if (SessionData.ContainsKey(name)) {
                return (T)SessionData[name];
            }
            throw new KeyNotFoundException("Unknown key: " + name);
        }

        /// <summary>
        /// Określa czy wartość o podanej nazwie jest zapisana w sesji
        /// </summary>
        /// <param name="name">Nazwa do sprawdzenia</param>
        /// <returns>True jeśli zapisana w sesji, False w przeciwnym wypadku</returns>
        public bool Has(string name) {
            return SessionData.ContainsKey(name);
        }

        /// <summary>
        /// Usuwa wartość z sesji
        /// </summary>
        /// <param name="name">Nazwa wartości do usunięcia</param>
        /// <returns>True, jeśli wartość była w sesji, False w przeciwnym wypadku</returns>
        public bool Remove(string name) {
            if (SessionData.ContainsKey(name)) {
                SessionData.Remove(name);
                return true;
            }
            return false;
        }
    }
}
