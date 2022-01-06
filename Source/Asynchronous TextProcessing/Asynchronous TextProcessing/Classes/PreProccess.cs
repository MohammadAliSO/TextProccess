using Asynchronous_TextProcessing.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace Asynchronous_TextProcessing.Classes
{
    public class PreProccess
    {
        DbContextOptionsBuilder<DBContext> optionsBuilder = new DbContextOptionsBuilder<DBContext>();
        public PreProccess()
        {
            optionsBuilder.UseSqlServer(Config.All.ConnectionStrings.DefaultConnection);
        }

        public async Task<bool> ExecuteAsync(RequestT req)
        {
            return await Task.Run(async () =>
            {
                using (DBContext _context = new(optionsBuilder.Options))
                {
                    try
                    {
                        //set state in db
                        var startProccess = DateTime.Now;
                        //req.StartDateTime = DateTime.Now;
                        req.State = (int)RequestState.InProcess;
                        _context.RequestTs.Update(req);
                        _context.SaveChanges();

                        string text = JsonConvert.DeserializeObject<TextProcessRequestModel>(req.RequestData).Text;
                        //Removal punctautions and URLs
                       var textProccessed= RemovePunctuations(text);
                        //Lowering the text
                        textProccessed = textProccessed.ToLower();
                        //Tokenization
                        var textTokenized =Tokenization(textProccessed);
                        var EndProccess = DateTime.Now;
                        req.Result = new ResultT
                        {
                            EndProcessTime = EndProccess,
                            StartProcessTime = startProccess,
                            ResultData = JsonConvert.SerializeObject(textTokenized),
                            Description = (EndProccess - startProccess).TotalSeconds + "Sec",
                            Type = (byte)RequestType.TextProcessing
                        };
                        req.State = (int)RequestState.Done;
                        //req.State = isComplete ? (byte)RequestState.Done : isFailure ? (byte)RequestState.Failure : (byte)RequestState.Incomplete;
                        _context.RequestTs.Update(req);
                        _context.SaveChanges();

                        ////req.EndDateTime = DateTime.Now;
                        //_context.RequestTs.Update(req);
                        //_context.SaveChanges();
                        return true;
                    }
                    catch (Exception e)
                    {
                        req.State = (int)RequestState.Failure;
                        _context.RequestTs.Update(req);
                        _context.SaveChanges();
                        return false;
                    }
                }
                
            });
        }

        private string RemovePunctuations(string text)
        {
            var sb = new StringBuilder();
            string cleanedText = Regex.Replace(text, @"(http|https):[^\s]+", "");
            foreach (char c in cleanedText)
            {
                if (!char.IsPunctuation(c))
                    sb.Append(c);
                else
                {
                    if(c is '.') sb.Append('\n');
                    else sb.Append(' ');
                }
            }
            return sb.ToString();
        }
        private List<List<string>> Tokenization(string text)
        {
            var textLines = text.Split('\n');

            List<List<string>> text_LinesWord = new List<List<string>>();
            for (int i = 0; i < textLines.Length ; i++)
            {
                text_LinesWord.Add(new List<string>());
                text_LinesWord[i]= textLines[i].Split(' ').ToList();
            } 
            return text_LinesWord;
        }
    }
}
