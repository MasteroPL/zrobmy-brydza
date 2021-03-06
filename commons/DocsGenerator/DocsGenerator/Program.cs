﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocsGenerator.Utils;

namespace DocsGenerator {
    class Program {
        static void Main(string[] args) {
            var docs = new XMLDocs();
            docs.ProcessXML("_source\\EasyHosting.xml");
            docs.WriteToFiles();

            var docs2 = new XMLDocs();
            docs2.ProcessXML("_source\\ServerSocket.xml");
            docs2.WriteToFiles();
        }
    }
}
