from os import listdir
from os.path import isfile, join
from utils.process_xml_doc import XMLDocs

SOURCE_PATH = "_source"
OUTPUT_PATH = "_output"

onlyfiles = [f for f in listdir(SOURCE_PATH) if isfile(join(SOURCE_PATH, f))]

for file_path in onlyfiles:
    XMLDocs.process_xml_file(join(SOURCE_PATH, file_path))

print("OK")
