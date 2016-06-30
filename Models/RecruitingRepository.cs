using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FrontwiseRecruitingApp.Models
{
    public class RecruitingRepository
    {
        private RecruitingAppContext entity = new RecruitingAppContext();

        public List<Category> GetAllCategories()
        {
            return entity.Categories.ToList();
        }

        public List<WordsDto> GetAllWords()
        {
            var listWords = entity.Words.Select(x => new WordsDto { ID = x.ID, Word = x.Word, CategoryID = x.CategoryID, WordsGroup = x.WordsGroup, Available = x.Available, PostedBy = x.PostedBy }).Where(x => x.Available == true);
            return listWords.ToList();
        }

        public List<WordsDto> GetEditorWords()
        {
            var listWords = entity.Words.Select(x => new WordsDto { ID = x.ID, Word = x.Word, CategoryID = x.CategoryID, WordsGroup = x.WordsGroup, Available = x.Available, PostedBy = x.PostedBy }).Where(x => x.Available == false);
            return listWords.ToList();
        }

        public List<SearchStringsDto> GetEditorSearchStrings()
        {
            var listSearchStrings = entity.SearchStrings.Select(x => new SearchStringsDto { ID = x.ID, SearchString = x.SearchString, CategoryID = x.CategoryID, SearchStringGroupID = x.SearchStringGroupID, Available = x.Available, PostedBy = x.PostedBy }).Where(x => x.Available == false);
            return listSearchStrings.ToList();
        }

        public List<SearchStringsDto> GetAllSearchStrings()
        {
            var listSearchStrings = entity.SearchStrings.Select(x => new SearchStringsDto { ID = x.ID, SearchString = x.SearchString, CategoryID = x.CategoryID, SearchStringGroupID = x.SearchStringGroupID, Available = x.Available, PostedBy = x.PostedBy }).Where(x => x.Available == true);
            return listSearchStrings.ToList();
        }
        public List<WordsDto> GetWordsByCategory(string category)
        {
            var listWords = entity.Words.Select(x => new WordsDto { ID = x.ID, Word = x.Word, CategoryID = x.CategoryID, WordsGroup = x.WordsGroup, Available = x.Available, PostedBy = x.PostedBy }).Where(x => x.CategoryID == category && x.Available == true);
            return listWords.ToList();
        }

        public List<WordsDto> WordsWithCategory(string category, string group)
        {
            var listWords = entity.Words.Select(x => new WordsDto { ID = x.ID, Word = x.Word, CategoryID = x.CategoryID, WordsGroup = x.WordsGroup, Available = x.Available, PostedBy = x.PostedBy }).Where(x => x.CategoryID == category && x.WordsGroup == group);
            return listWords.ToList();
        }

        public List<SearchStringsDto> GetSearchStringsByCategory(string category)
        {
            var listSearchStrings = entity.SearchStrings.Select(x => new SearchStringsDto { ID = x.ID, SearchString = x.SearchString, CategoryID = x.CategoryID, SearchStringGroupID = x.SearchStringGroupID, Available = x.Available, PostedBy = x.PostedBy }).Where(x => x.CategoryID == category && x.Available == true);
            return listSearchStrings.ToList();
        }

        public List<SearchStringsDto> GetSimilarStrings(string category, string group)
        {
            var listSearchStrings = entity.SearchStrings.Select(x => new SearchStringsDto { ID = x.ID, SearchString = x.SearchString, CategoryID = x.CategoryID, SearchStringGroupID = x.SearchStringGroupID, Available = x.Available, PostedBy = x.PostedBy }).Where(x => x.CategoryID == category && x.SearchStringGroupID == group);
            return listSearchStrings.ToList();
        }

        public List<SearchStringsDto> GetUserSearchStrings(string userid)
        {
            var listSearchStrings = entity.SearchStrings.Select(x => new SearchStringsDto { SearchString = x.SearchString, CategoryID = x.CategoryID, SearchStringGroupID = x.SearchStringGroupID, Available = x.Available, PostedBy = x.PostedBy }).Where(x => x.PostedBy == userid && x.Available == true);
            return listSearchStrings.ToList();
        }

        public List<WordsDto> GetUserWords(string userid)
        {
            var listWords = entity.Words.Select(x => new WordsDto { Word = x.Word, CategoryID = x.CategoryID, WordsGroup = x.WordsGroup, Available = x.Available, PostedBy = x.PostedBy }).Where(x => x.PostedBy == userid && x.Available == true);
            return listWords.ToList();
        }

        public List<UserDto> GetUser(string email)
        {
            var user = entity.User.Select(x => new UserDto { ID = x.ID, email = x.email, Name = x.Name, Position = x.Position, Role = x.Role, Surname = x.Surname, user_password = x.user_password, Points = x.Points, profile_photo_url = x.profile_photo_url }).Where(x => x.email == email);

            return user.ToList();
        }

        public List<UserDto> GetUsers()
        {
            var users = entity.User.Select(x => new UserDto { ID = x.ID, email = x.email, Name = x.Name, Position = x.Position, Role = x.Role, Surname = x.Surname, Points = x.Points, profile_photo_url = x.profile_photo_url });
            return users.ToList();
        }

        public int lastSynonymGroupID()
        {
            var words = entity.Words.OrderByDescending(x => x.WordsGroup).First();
            return Convert.ToInt32(words.WordsGroup);
        }

        public int lastSearchStringGroupID()
        {
            var searchstrings = entity.SearchStrings.OrderByDescending(x => x.SearchStringGroupID).First();
            return Convert.ToInt32(searchstrings.SearchStringGroupID);
        }

        public void AddWord(WordsCategory word)
        {
            var editedWord = entity.Words.Find(word.ID);
            if (editedWord == null)
            {
                if (word.WordsGroup == null)
                {
                    int lastID = lastSynonymGroupID();
                    lastID = lastID + 1;
                    string LastID = lastID.ToString();
                    word.WordsGroup = LastID;
                }
                entity.Words.Add(word);
            }
            else {
                addPoints(word.PostedBy);
                editedWord.Word = word.Word;
                editedWord.CategoryID = word.CategoryID;
                editedWord.WordsGroup = word.WordsGroup;
                editedWord.Available = word.Available;
                editedWord.PostedBy = word.PostedBy;
            }
            entity.SaveChanges();
        }

        public void AddSearchString(SearchStringsCategory sString)
        {

            var editedString = entity.SearchStrings.Find(sString.ID);
            if (editedString == null)
            {
                if (sString.SearchStringGroupID == null)
                {
                    int lastID = lastSearchStringGroupID();
                    lastID = lastID + 1;
                    string LastID = lastID.ToString();
                    sString.SearchStringGroupID = LastID;
                }
                entity.SearchStrings.Add(sString);
            }
            else {
                addPoints(sString.PostedBy);
                editedString.SearchString = sString.SearchString;
                editedString.CategoryID = sString.CategoryID;
                editedString.SearchStringGroupID = sString.SearchStringGroupID;
                editedString.Available = sString.Available;
                editedString.PostedBy = sString.PostedBy;
            }
            entity.SaveChanges();
        }
        public void UpdateUser(User user)
        {
            var editedUser = entity.User.Find(user.ID);
            if (editedUser == null)
            {
                user.user_password = "123456";
                entity.User.Add(user);
            }
            else {
                editedUser.Name = user.Name;
                editedUser.Surname = user.Surname;
                editedUser.Role = user.Role;
                editedUser.Position = user.Position;
                editedUser.email = user.email;
                editedUser.profile_photo_url = user.profile_photo_url;
                if (user.user_password != null)
                {
                    editedUser.user_password = user.user_password;
                }
            }
            entity.SaveChanges();
        }

        public void addPoints(string userId)
        {
            int user = Int16.Parse(userId);
            var User = entity.User.Find(user);
            User.Points = User.Points + 2;
            entity.SaveChanges();
        }

        public string addCatchedUser(UsersSaved user)
        {
            entity.UsersSaved.Add(user);
            entity.SaveChanges();

            return "saved";
        }

        public void RemoveUser(User user)
        {
            var User = new User { ID = user.ID };
            entity.User.Attach(User);
            entity.User.Remove(User);
            entity.SaveChanges();
        }

        public void RemoveWord(WordsCategory word)
        {
            var Word = new WordsCategory { ID = word.ID };
            entity.Words.Attach(Word);
            entity.Words.Remove(Word);
            entity.SaveChanges();
        }

        public void RemoveSearchString(SearchStringsCategory SearchString)
        {
            var searchString = new SearchStringsCategory { ID = SearchString.ID };
            entity.SearchStrings.Attach(searchString);
            entity.SearchStrings.Remove(searchString);
            entity.SaveChanges();
        }

        public string LoginUser(string email, string pass)
        {
            var user = entity.User.Select(x => new UserDto { ID = x.ID, email = x.email, Name = x.Name, Position = x.Position, Role = x.Role, Surname = x.Surname, user_password = x.user_password, Points = x.Points, profile_photo_url = x.profile_photo_url }).Where(x => x.email == email).FirstOrDefault();
            if (user == null)
            {
                return "Email not found!";
            }
            else {
                if (user.user_password == pass)
                {
                    user.user_password = null;
                    return JsonConvert.SerializeObject(user);
                }
                else
                {
                    return "Invalid password";
                }
            }
        }

        public void calculateEditors()
        {
            var wordsPending = entity.Words.Select(x => new WordsDto { ID = x.ID }).Where(x => x.Available == false).Count();
            var editorsNum = entity.User.Select(x => new UserDto { ID = x.ID }).Where(x => x.Role == "Editor").Count();

        }

        public List<SearchHistoryDto> getHistory(string userid)
        {
            var historyData = entity.SearchHistory.Select(x => new SearchHistoryDto { ID = x.ID, UserID = x.UserID, SearchID = x.SearchID, SearchLabel = x.SearchLabel, SubmitDate = x.SubmitDate, GroupID = x.GroupID }).Where(x => x.UserID == userid).OrderByDescending(x => x.ID);
            return historyData.ToList();
        }

        public string saveSearch(SearchHistory search)
        {
            entity.SearchHistory.Add(search);
            entity.SaveChanges();
            return "saved";

        }
    }
}