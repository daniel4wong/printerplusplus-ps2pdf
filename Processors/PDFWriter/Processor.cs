using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrinterPlusPlusSDK;
using Ghostscript.NET.Processor;
using Ghostscript.NET;
using System.IO;

namespace PDFWriter
{
    public class Processor : PrinterPlusPlusSDK.IProcessor
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(Processor));

        //https://www.ghostscript.com/download/gsdnld.html

        public ProcessResult Process(string key, string psFilename)
        {
            log.DebugFormat("PDFWriter process: key={0}, psFilename={1}", key, psFilename);

            GhostscriptPipedOutput gsPipedOutput = new GhostscriptPipedOutput();

            // pipe handle format: %handle%hexvalue
            string outputPipeHandle = "%handle%" + int.Parse(gsPipedOutput.ClientHandle).ToString("X2");

            FileInfo fileInfo = new FileInfo(psFilename);
            var outputFilename = Path.Combine(fileInfo.Directory.FullName,
                string.Format("{0}.pdf", Path.GetFileNameWithoutExtension(fileInfo.Name)));

            try
            {
                using (GhostscriptProcessor processor = new GhostscriptProcessor())
                {
                    List<string> switches = new List<string>();
                    switches.Add("-empty");
                    switches.Add("-dQUIET");
                    switches.Add("-dSAFER");
                    switches.Add("-dBATCH");
                    switches.Add("-dNOPAUSE");
                    switches.Add("-dNOPROMPT");
                    switches.Add("-sDEVICE=pdfwrite");
                    switches.Add("-o" + outputPipeHandle);
                    switches.Add("-q");
                    switches.Add("-f");
                    switches.Add(psFilename);

                    try
                    {
                        processor.StartProcessing(switches.ToArray(), null);
                        byte[] rawDocumentData = gsPipedOutput.Data;
                        File.WriteAllBytes(outputFilename, rawDocumentData);

                        log.DebugFormat("PDFWriter success: {0}", outputFilename);
                    }
                    catch (Exception ex)
                    {
                        log.Error("PDFWriter failed", ex);
                    }
                    finally
                    {
                        gsPipedOutput.Dispose();
                        gsPipedOutput = null;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("PDFWriter failed", ex);
            }

            return new ProcessResult();
        }
    }
}
