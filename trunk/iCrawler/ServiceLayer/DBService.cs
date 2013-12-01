using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iCrawler.Models;

namespace iCrawler.ServiceLayer
{
    public class DbHelper
    {
        private iCrawlerEntities db = new iCrawlerEntities();

        public Dictionary<string, string> GetContacts(string url, string htmlContent)
        {
            List<int> _contacts = new List<int>();
            Url _url = new Url();
            _url = db.Urls.Where(c => url.Contains(c.Url1)).FirstOrDefault();
            if(_url == null) return null;

            Guid _urlId = _url.Id;

            _contacts = db.UrlOfUsers.Where(t => t.UrlId == _urlId).Select(c => c.UserId).ToList();

            Dictionary<string, string> _recipients = new Dictionary<string, string>();

            foreach (int _userId in _contacts)
            {
                string _keywords = db.UrlOfUsers.Where(c => c.UserId == _userId && c.UrlId == _urlId).FirstOrDefault().Keywords;
                string _outKeywords = db.UrlOfUsers.Where(c => c.UserId == _userId && c.UrlId == _urlId).FirstOrDefault().OutKeywords;
                string _email = db.UrlOfUsers.Where(c => c.UserId == _userId && c.UrlId == _urlId).FirstOrDefault().Emails;

                FilterService _filterService = new FilterService();
                _filterService.InKeywords = _keywords;
                _filterService.OutKeywords = _outKeywords;
                _filterService.Text = htmlContent;

                if (_keywords != null && _email != null && _filterService.IsPassFilter())
                {
                    if (_email.Contains(",") || _email.Contains(";"))
                    {
                        string[] strSplits = new string[] { ",", ";" };
                        string[] arrEs = _email.Split(strSplits, StringSplitOptions.None);
                        if (arrEs != null && arrEs.Count() > 1)
                        {
                            foreach (var _e in arrEs)
                            {
                                if (_e.Contains("@"))
                                {
                                    _recipients.Add(_e, _e);
                                }
                            }
                        }
                    }
                    else
                    {
                        _recipients.Add(_email, _email);
                    }
                }
            }

            return _recipients;
        }

        public bool IsExitsUrl(string url)
        {
            CurrentLink _link = new CurrentLink();
            _link = db.CurrentLinks.Where(c => c.Url.Contains(url)).FirstOrDefault();
            if (_link != null)
                return true;
            else
                return false;
        }

        public HistoryLink CurrentLinkToLink(CurrentLink link)
        {
            return new HistoryLink
            {
                Id = link.Id,
                Url = link.Url,
                DownloadTime = link.DownloadTime,
                HtmlContent = link.HtmlContent,
                TextContent = link.TextContent,
                Title = link.Title
            };
        }

        public void CleanDB(string url)
        {
            string _lastUrl = "";
            int _maxId = 0;
            bool _isClean = true;
            List<CurrentLink> _listToMove = new List<CurrentLink>();

            var _lastLink = db.CurrentLinks.Where(c => c.Url.Contains(url)).OrderByDescending(c => c.Url).FirstOrDefault();
            if (db.CurrentLinks.Where(c => c.Url.Contains(url)).Count() <= 10) _isClean = false;

            if ((_lastLink != null) && _isClean)
            {
                //_currentMaxId
                _lastUrl = _lastLink.Url;
                string[] strEs = _lastUrl.Split(new string[] { "/" }, StringSplitOptions.None);

                _maxId = int.Parse(strEs[strEs.Length - 1]);

                //Link to Move
                string currentMaxId = _maxId.ToString();
                DateTime _pointToDelete = DateTime.Now.AddDays(-1);
                _listToMove = db.CurrentLinks.Where(c => c.CreateDate.Value < _pointToDelete).ToList();

                MoveLinksToHistory(_listToMove);
            }
        }

        public void CleanHistory()
        {
            List<Url> _urls = new List<Url>();
            _urls = db.Urls.ToList();
            if (_urls != null && _urls.Count > 1)
            {
                foreach (Url _url in _urls)
                {
                    CleanHistory(_url.Url1);
                }
            }
        }

        public bool CleanHistory(string url)
        {
            DateTime _pointToDelete = DateTime.Now.AddDays(-7);
            List<HistoryLink> _histories = new List<HistoryLink>();
            _histories = db.HistoryLinks.Where(c => c.Url == url && c.CreateDate < _pointToDelete).ToList();

            if (_histories != null && _histories.Count > 1)
            {
                foreach (HistoryLink _his in _histories)
                {
                    db.HistoryLinks.Remove(_his);
                    try
                    {
                        db.SaveChanges();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void CleanDB()
        {
            List<Url> _urls = new List<Url>();
            _urls = db.Urls.ToList();

            if (_urls != null && _urls.Count > 0)
            {
                foreach (var _url in _urls)
                {
                    CleanDB(_url.Url1);
                    CleanHistory(_url.Url1);
                }
            }
        }

        public void MoveLinksToHistory(List<CurrentLink> links)
        {
            foreach (var link in links)
            {
                HistoryLink _link = CurrentLinkToLink(link);
                db.HistoryLinks.Add(_link);
                db.CurrentLinks.Remove(link);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
