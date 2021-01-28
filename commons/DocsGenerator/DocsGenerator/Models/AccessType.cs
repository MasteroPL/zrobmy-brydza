using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsGenerator.Models {
    public enum AccessType {
        PRIVATE = '-',
        PROTECTED = '#',
        INNER = '_',
        PUBLIC = '+'
    }

    public static class AccessTypeM {
        public static string GetName(AccessType at) {
            switch (at) {
                case AccessType.PRIVATE:
                    return "private";
                case AccessType.PROTECTED:
                    return "protected";
                case AccessType.PUBLIC:
                    return "public";
                case AccessType.INNER:
                    return "";
            }

            return null;
        }
    }
}
