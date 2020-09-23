# ASP.NET Zero Language Translation Utility
CLI utility to produce language translations of ASP.NET Zero .xml language files using Google Translate API

Dependency on the Google Tranlsation v2 API, please review...
* https://cloud.google.com/translate/docs/setup
* https://cloud.google.com/docs/authentication/getting-started

Once you have your service account key (in .json format), update your environment to include the GOOGLE_APPLICATION_CREDENTIALS pointer as described in the help links above.

All xml files (source and translated files) will be stored in the ./xml/ directory. A sample portal.xml file is included in the ./xml/ directory with a cut-down list of entries for testing

Command line options for the utility are...
* -l <languageCode> = sets the translation language using google recognised language codes (for example fr = french, it = italian, ja = japanese, etc.)
* -t <filename.ext? = sets the filename for the translated xml file, defaults to <sourceFilename>-<targetLanguage>.xml (for example portal-fr.xml)
* -s <languageCode> = sets the source language, defaults to en (English)
* -f <filename.ext> = sets the source filename, defaults to portal.xml

`LanguageTranslation.exe -l fr` will translate the source file portal.xml from English to French and save the translated file as portal-fr.xml
