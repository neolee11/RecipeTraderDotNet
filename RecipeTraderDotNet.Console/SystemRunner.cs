using System;
using System.Collections.Generic;
using System.Linq;
using RecipeTraderDotNet.Core.Application;
using RecipeTraderDotNet.Core.Common;
using RecipeTraderDotNet.Core.Domain.Market;
using RecipeTraderDotNet.Core.Domain.Recipe;
using RecipeTraderDotNet.Core.Domain.Repositories;
using RecipeTraderDotNet.Core.Domain.User;
using RecipeTraderDotNet.Data.Repositories.Memory;

namespace RecipeTraderDotNet.Console
{
    public class SystemRunner
    {
        private const string ErrMsgLoginRequired = "Please login to perform this action.";
        private IPrivateRecipeRepository _privateRecipeRepository;
        private IPublicRecipeRepository _publicRecipeRepository;
        private IMoneyAccountRepository _moneyAccountRepository;

        private List<MoneyAccount> _moneyAccounts;
        private List<PublicRecipe> _marketRecipes;
        private List<PrivateRecipe> _userRecipes;
        private IMarket _market;

        private string _currentUser;
        private UserService _userService;

        private readonly Random _random = new Random();
      
        /// <summary>
        /// Intial System consists of:
        /// Three user: daniel with $120, jennie with $100, tom with $80
        /// Private Recipes: daniel has 2, jennie has 1, tom has 1 purchased from the market
        /// Public reicpes: daniel has 1 with price of 20, with review from tom
        /// </summary>
        public void InitializeSystem()
        {
            var userDaniel = DomainObjectsGenerator.GenerateRandoMoneyAccount("daniel");
            var userJennie = DomainObjectsGenerator.GenerateRandoMoneyAccount("jennie");
            var userTom = DomainObjectsGenerator.GenerateRandoMoneyAccount("tom");

            var recipeDaniel1 = DomainObjectsGenerator.GenerateRandomPrivateRecipe(4, userDaniel.UserId);
            var recipeDaniel2 = DomainObjectsGenerator.GenerateRandomPrivateRecipe(2, userDaniel.UserId);
            var recipeJennie1 = DomainObjectsGenerator.GenerateRandomPrivateRecipe(5, userJennie.UserId);

            var recipeOnMarket = PublicRecipe.ConvertFromPrivateRecipe(recipeDaniel1);
            recipeOnMarket.Id = GetRandomInt();
            if (recipeOnMarket.Items != null)
            {
                foreach (var recipeItem in recipeOnMarket.Items)
                {
                    recipeItem.Id = GetRandomInt();
                }
            }
            recipeOnMarket.Price = 20;
            recipeOnMarket.TimePublished = new DateTime(2016, 3, 20, 11, 0, 0);

            var tomsReview = DomainObjectsGenerator.GenerateRandomReview(recipeOnMarket);
            tomsReview.ReviewerUserId = userTom.UserId;
            tomsReview.Comment = "Good recipe";
            tomsReview.Rating = 4;
            recipeOnMarket.AddReview(tomsReview);
          
            _moneyAccounts = new List<MoneyAccount>
            {
                userDaniel,
                userJennie,
                userTom
            };

            _marketRecipes = new List<PublicRecipe>
            {
                recipeOnMarket
            };

            _moneyAccountRepository = new MoneyAccountRepository(_moneyAccounts);
            _publicRecipeRepository = new PublicRecipeRepository(_marketRecipes);

            _market = new Market(_publicRecipeRepository, _moneyAccountRepository);

            var recipeTom = _market.Purchase(recipeOnMarket.Id, "tom");
            recipeTom.PurchaseInformation.TimePurchased = new DateTime(2016, 4, 1, 9, 0, 0);

            _userRecipes = new List<PrivateRecipe>
            {
                recipeDaniel1,
                recipeDaniel2,
                recipeJennie1,
                recipeTom
            };
            _privateRecipeRepository = new PrivateRecipeRepository(_userRecipes);
        }

        private int GetRandomInt()
        {
            return _random.Next(1, Int32.MaxValue);
        }

        /// <summary>
        /// Command Pattern:
        /// commandType objectType [object id or name or value] -field [field value]
        /// </summary>
        /// <param name="rawCommand"></param>
        /// <returns>Output</returns>
        public string ProcessCommand(string rawCommand)
        {
            var output = "Invalid command. Type 'help' for system commands";
            if (string.IsNullOrWhiteSpace(rawCommand)) return output;

            var command = PreprocessCommand(rawCommand);

            switch (command.CommandType)
            {
                case CommandType.Help:
                    output = GetCommandHelp();
                    break;
                case CommandType.Login:
                    output = ProcessLoginCommand(command);
                    break;
                case CommandType.Show:
                    output = ProcessShowCommand(command);
                    break;
                case CommandType.Add:
                    output = ProcessAddCommand(command);
                    break;
                case CommandType.Edit:
                    output = ProcessEditCommand(command);
                    break;
                case CommandType.Remove:
                    output = ProcessRemoveCommand(command);
                    break;
                case CommandType.Publish:
                    output = ProcessPublishCommand(command);
                    break;
                case CommandType.Takedown:
                    output = ProcessTakeDownCommand(command);
                    break;
                case CommandType.Purchase:
                    output = ProcessPurchaseCommand(command);
                    break;
                case CommandType.Review:
                    output = ProcessReviewCommand(command);
                    break;
                default:
                    output = command.ToString();
                    break;
            }

            return output;
        }

        private string ProcessReviewCommand(Command command)
        {
            var output = "Invalid Review Command";

            if (!IsUserLogin()) return ErrMsgLoginRequired;

            if (command.MainObjPair.Key == DomainObjectType.PublicRecipe)
            {
                if (command.MainObjPair.Value == string.Empty)
                {
                    return "Review recipe must specify public recipe ID";
                }

                if (command.OptionalCommandPairs == null || command.OptionalCommandPairs.Count < 2)
                {
                    return "Review recipe must specify a rating and optionally with a comment";
                }

                var recipeIdPattern = command.MainObjPair.Value;
                int recipeId;
                if (int.TryParse(recipeIdPattern, out recipeId) == false)
                {
                    return "Review recipe must specify a valid recipe ID";
                }

                string ratingPattern = string.Empty;
                double rating;
                string comment = string.Empty;
                foreach (var optionalCommandPair in command.OptionalCommandPairs)
                {
                    var currValue = optionalCommandPair.Key.ToLower();
                    if (currValue.Contains("rating")) ratingPattern = optionalCommandPair.Value;
                    else if (currValue.Contains("comment")) comment = optionalCommandPair.Value;
                }

                if (double.TryParse(ratingPattern, out rating) == false)
                {
                    return "Review recipe must specify a valid rating";
                }

                var result = _userService.ReviewRecipe(recipeId, rating, comment);

                return string.IsNullOrEmpty(result) ? "Review recipe successfully" : result;
            }

            return output;
        }

        private string ProcessPurchaseCommand(Command command)
        {
            var output = "Invalid Purchase Command";

            if (!IsUserLogin()) return ErrMsgLoginRequired;

            if (command.MainObjPair.Key == DomainObjectType.PublicRecipe)
            {
                if (command.MainObjPair.Value == string.Empty)
                {
                    return "Purchase recipe must specify recipe ID";
                }

                var recipeIdPattern = command.MainObjPair.Value;
                int recipeId;
                if (int.TryParse(recipeIdPattern, out recipeId) == false)
                {
                    return "Purchase recipe must specify a valid recipe ID";
                }

                var result = _userService.PurchaseRecipe(recipeId);

                return string.IsNullOrEmpty(result) ? "Purchase recipe successfully" : result;
            }

            return output;
        }

        private string ProcessTakeDownCommand(Command command)
        {
            var output = "Invalid Takedown Command";

            if (!IsUserLogin()) return ErrMsgLoginRequired;

            if (command.MainObjPair.Key == DomainObjectType.PublicRecipe)
            {
                if (command.MainObjPair.Value == string.Empty)
                {
                    return "Takedown recipe must specify recipe ID";
                }

                var recipeIdPattern = command.MainObjPair.Value;
                int recipeId;
                if (int.TryParse(recipeIdPattern, out recipeId) == false)
                {
                    return "Takedown recipe must specify a valid recipe ID";
                }

                var result = _userService.TakeDownRecipe(recipeId);

                return string.IsNullOrEmpty(result) ? "Takedown recipe successfully" : result;
            }

            return output;
        }

        private string ProcessPublishCommand(Command command)
        {
            var output = "Invalid Publish Command";

            if (!IsUserLogin()) return ErrMsgLoginRequired;

            if (command.MainObjPair.Key == DomainObjectType.PrivateRecipe)
            {
                if (command.MainObjPair.Value == string.Empty)
                {
                    return "Publish recipe must specify recipe ID";
                }

                if (command.OptionalCommandPairs == null || !command.OptionalCommandPairs.Any())
                {
                    return "Publish recipe must specify price";
                }

                var recipeIdPattern = command.MainObjPair.Value;
                int recipeId;
                if (int.TryParse(recipeIdPattern, out recipeId) == false)
                {
                    return "Publish recipe must specify a valid recipe ID";
                }

                string pricePattern = string.Empty;
                decimal price;

                foreach (var optionalCommandPair in command.OptionalCommandPairs)
                {
                    var currValue = optionalCommandPair.Key.ToLower();
                    if (currValue.Contains("price")) pricePattern = optionalCommandPair.Value;
                }

                if (decimal.TryParse(pricePattern, out price) == false)
                {
                    return "Publish recipe must specify a valid price";
                }

                var recipe = _privateRecipeRepository.GetById(recipeId);
                if (recipe == null)
                {
                    return $"Recipe with ID {recipeId} not found";
                }

                var result = _userService.PublishRecipe(recipe.Id, price);
             
                return string.IsNullOrEmpty(result) ? "Publish recipe successfully" : result;
            }

            return output;
        }

        private string ProcessRemoveCommand(Command command)
        {
            var output = "Invalid remove command";

            if (!IsUserLogin()) return ErrMsgLoginRequired;

            if (command.MainObjPair.Key == DomainObjectType.PrivateRecipe)
            {
                return RemovePrivateRecipe(command);
            }

            if (command.MainObjPair.Key == DomainObjectType.RecipeItem)
            {
                return RemovePrivateRecipeItem(command);
            }

            return output;
        }

        private string RemovePrivateRecipeItem(Command command)
        {
            if (command.MainObjPair.Value == string.Empty)
            {
                return "Remove item must specify item ID";
            }

            if (command.OptionalCommandPairs == null || !command.OptionalCommandPairs.Any())
            {
                return "Remove item must specify recipe ID";
            }

            var itemIdPattern = command.MainObjPair.Value;
            int itemId;
            if (int.TryParse(itemIdPattern, out itemId) == false)
            {
                return "Remove item must specify a valid item ID";
            }

            string recipeIdPattern = string.Empty;
            int recipeId;

            foreach (var optionalCommandPair in command.OptionalCommandPairs)
            {
                var currValue = optionalCommandPair.Key.ToLower();
                if (currValue.Contains("recipe")) recipeIdPattern = optionalCommandPair.Value;
            }

            if (int.TryParse(recipeIdPattern, out recipeId) == false)
            {
                return "Remove item must specify a valid recipe ID";
            }

            var recipe = _privateRecipeRepository.GetById(recipeId);
            if (recipe == null)
            {
                return $"Recipe with ID {recipeId} not found";
            }

            var item = recipe.Items.SingleOrDefault(i => i.Id == itemId);
            if (item == null)
            {
                return $"Recipe item ID {itemId} not found in the recipe";
            }

            recipe.Remove(item);
            _privateRecipeRepository.Update(recipe);
            return "Remove item successfully";
        }

        private string RemovePrivateRecipe(Command command)
        {
            var recipePattern = command.MainObjPair.Value;
            if (string.IsNullOrEmpty(recipePattern))
            {
                return "Edit recipe must specify recipe ID or pattern";
            }

            int recipeId;
            PrivateRecipe recipe = null;
            if (int.TryParse(recipePattern, out recipeId))
            {
                //search by id
                recipe = _userService.GetUserRecipes().SingleOrDefault(r => r.Id == recipeId);
            }
            else
            {
                //search by title
                var foundRecipes = _userService.GetUserRecipes().Where(r => r.Title.ToLower().Contains(recipePattern.ToLower())).ToList();
                if (foundRecipes.Count > 1)
                {
                    return $"{foundRecipes.Count} recipes with title {recipePattern} found. Cannot Edit. Title must be unique\n";
                }

                if (foundRecipes.Count == 1)
                {
                    recipe = foundRecipes[0];
                }
            }

            if (recipe == null)
            {
                return "Recipe not found";
            }


            _privateRecipeRepository.Delete(recipe.Id);

            var output = $"Delete recipe {recipe.Id} : {recipe.Title} successful";
            return output;
        }

        private string ProcessEditCommand(Command command)
        {
            var output = "Invalid edit command";

            if (!IsUserLogin()) return ErrMsgLoginRequired;

            if (command.MainObjPair.Key == DomainObjectType.PrivateRecipe)
            {
                return EditPrivateRecipe(command);
            }

            if (command.MainObjPair.Key == DomainObjectType.RecipeItem)
            {
                return EditPrivateRecipeItem(command);
            }

            return output;
        }

        private string EditPrivateRecipeItem(Command command)
        {
            if (command.MainObjPair.Value == string.Empty)
            {
                return "Edit item must specify item ID";
            }

            if (command.OptionalCommandPairs == null || command.OptionalCommandPairs.Count < 2)
            {
                return "Edit item must specify recipe ID and/or item description and/or status";
            }

            var itemIdPattern = command.MainObjPair.Value;
            int itemId;
            if (int.TryParse(itemIdPattern, out itemId) == false)
            {
                return "Edit item must specify a valid item ID";
            }

            string recipeIdPattern = string.Empty;
            int recipeId;
            string newDescription = string.Empty;
            string newStatus = string.Empty;

            foreach (var optionalCommandPair in command.OptionalCommandPairs)
            {
                var currValue = optionalCommandPair.Key.ToLower();
                if (currValue.Contains("recipe")) recipeIdPattern = optionalCommandPair.Value;
                else if (currValue.Contains("desc")) newDescription = optionalCommandPair.Value;
                else if (currValue.Contains("status")) newStatus = optionalCommandPair.Value;
            }

            if (int.TryParse(recipeIdPattern, out recipeId) == false)
            {
                return "Edit item must specify a valid recipe ID";
            }

            var recipe = _privateRecipeRepository.GetById(recipeId);
            if (recipe == null)
            {
                return $"Recipe with ID {recipeId} not found";
            }

            var item = recipe.Items.SingleOrDefault(i => i.Id == itemId);
            if (item == null)
            {
                return $"Recipe item ID {itemId} not found in the recipe";
            }

            if (newDescription != string.Empty)
            {
                item.Description = newDescription;
            }

            if (newStatus.ToLower() == RecipeItemStatus.New.ToString().ToLower())
            {
                item.Reset();
            }
            else if (newStatus.ToLower() == RecipeItemStatus.Done.ToString().ToLower())
            {
                item.Finish();
            }

            _privateRecipeRepository.Update(recipe);
            return "Edit item successfully";
        }

        private string EditPrivateRecipe(Command command)
        {
            var output = "";

            var recipePattern = command.MainObjPair.Value;
            if (string.IsNullOrEmpty(recipePattern))
            {
                return "Edit recipe must specify recipe ID or pattern";
            }

            if (command.OptionalCommandPairs == null || command.OptionalCommandPairs.Count == 0)
            {
                return "Edit recipe must associate with -title";
            }

            var newTitle = command.OptionalCommandPairs[0].Value;
            if (string.IsNullOrEmpty(newTitle)) return "New title in edit recipe must not be empty";

            int recipeId;
            PrivateRecipe recipe = null;
            if (int.TryParse(recipePattern, out recipeId))
            {
                //search by id
                recipe = _userService.GetUserRecipes().SingleOrDefault(r => r.Id == recipeId);
            }
            else
            {
                //search by title
                var foundRecipes = _userService.GetUserRecipes().Where(r => r.Title.ToLower().Contains(recipePattern.ToLower())).ToList();
                if (foundRecipes.Count == 1)
                {
                    recipe = foundRecipes[0];
                }
                else if (foundRecipes.Count > 1)
                {
                    output = $"{foundRecipes.Count} recipes with title {recipePattern} found. Editing first one : {foundRecipes[0].Title}\n";
                    recipe = foundRecipes[0];
                }
            }

            if (recipe == null)
            {
                return "Recipe not found";
            }

            recipe.Title = newTitle;

            _privateRecipeRepository.Update(recipe);
            output += $"Edit recipe {recipe.Id} : {recipe.Title} successful";
            return output;
        }

        private string ProcessAddCommand(Command command)
        {
            var output = "Invalid add command";

            if (command.MainObjPair.Key == DomainObjectType.User)
            {
                output = _market.CreateUserMoneyAccount(command.MainObjPair.Value);
            }
            else
            {
                if (IsUserLogin())
                {
                    if (command.MainObjPair.Key == DomainObjectType.PrivateRecipe)
                    {
                        return AddPrivateRecipe(command);

                    }
                    else if (command.MainObjPair.Key == DomainObjectType.RecipeItem)
                    {
                        return AddPrivateRecipeItem(command);
                    }
                }
                else
                {
                    output = ErrMsgLoginRequired;
                }
            }

            return output;
        }

        private string AddPrivateRecipe(Command command)
        {
            if (command.MainObjPair.Value == string.Empty)
            {
                return "Invalid recipe name";
            }
            else
            {
                var result = _userService.CreateNewPrivateRecipe(command.MainObjPair.Value);
                return result == string.Empty ? "Recipe created successfully" : result;
            }
        }

        private string AddPrivateRecipeItem(Command command)
        {
            var itemDesc = command.MainObjPair.Value;
            if (string.IsNullOrEmpty(itemDesc)) return "Item cannot be empty";

            if (command.OptionalCommandPairs == null || command.OptionalCommandPairs.Count == 0)
            {
                return "Add item must associate with a recipe";
            }

            int recipeId;
            if (int.TryParse(command.OptionalCommandPairs[0].Value, out recipeId) == false)
            {
                return "Add item must specify a recipe ID";
            }

            var recipe = _privateRecipeRepository.GetById(recipeId);
            if (recipe == null) return $"Recipe with ID [{recipeId}] not found";

            var item = new RecipeItem(itemDesc, recipe);
            recipe.Add(item);
            _privateRecipeRepository.Update(recipe);
            return "Item created successfully";
        }

 

        private string ProcessLoginCommand(Command command)
        {
            if (command.MainObjPair.Key == DomainObjectType.User)
            {
                var userId = command.MainObjPair.Value.ToLower();
                if (string.IsNullOrEmpty(userId))
                {
                    return "Please specify user id";
                }

                var userAccount = _moneyAccountRepository.GetUserMoneyAccount(userId);
                if (userAccount == null)
                {
                    return $"User {userId} does not exist";
                }

                _currentUser = userId;
                _userService = new UserService(userId, _market, _privateRecipeRepository, _publicRecipeRepository, _moneyAccountRepository);
                return $"Login user {userId} successful";
            }

            return "Invalid login command";
        }

        private string ProcessShowCommand(Command command)
        {
            if (command.MainObjPair.Key == DomainObjectType.SystemStatus)
            {
                return _market.GetSystemInfo().ToString();
            }

            if (command.MainObjPair.Key == DomainObjectType.PublicRecipe)
            {
                var publicRecipes = _market.GetAllRecipes();
                return publicRecipes.Print();
            }

            if (IsUserLogin())
            {
                if (command.MainObjPair.Key == DomainObjectType.UserAccountBalance)
                {
                    return _userService.GetUserMoneyAccount().ToString();
                }

                if (command.MainObjPair.Key == DomainObjectType.PrivateRecipe)
                {
                    if (string.IsNullOrEmpty(command.MainObjPair.Value)) //show all recipes
                    {
                        return ShowAllUserRecipes();
                    }
                    else
                    {
                        //show individual recipe
                        return ShowIndividualUserRecipe(command);
                    }
                }
            }
            else
            {
                return ErrMsgLoginRequired;
            }

            return "Invalid show command";
        }

        private string ShowIndividualUserRecipe(Command command)
        {
            var allUserRecipes = _userService.GetUserRecipes();

            var recipePattern = command.MainObjPair.Value;
            int recipeId;

            if (int.TryParse(recipePattern, out recipeId))
            {
                var selectedRecipe = allUserRecipes.SingleOrDefault(r => r.Id == recipeId);
                if (selectedRecipe == null) return $"Your recipe with id {recipeId} does not exist";
                else
                {
                    if (command.OptionalCommandPairs != null && command.OptionalCommandPairs.Any())
                    {
                        if (command.OptionalCommandPairs[0].Key.Contains("status"))
                        {
                            selectedRecipe = selectedRecipe.Copy();
                            selectedRecipe.Items = selectedRecipe.Items.Where(i => i.Status.ToString().ToLower() == command.OptionalCommandPairs[0].Value).ToList();
                        }
                    }
                    return selectedRecipe.ToString();
                }
            }
            else
            {
                //title
                var selectedRecipes = allUserRecipes.Where(r => r.Title.ToLower().Contains(recipePattern.ToLower())).ToList();
                if (!selectedRecipes.Any()) return $"Your recipe with title pattern {recipePattern} does not exist";

                selectedRecipes = selectedRecipes.Copy();
                if (command.OptionalCommandPairs != null && command.OptionalCommandPairs.Any())
                {
                    if (command.OptionalCommandPairs[0].Key.Contains("status"))
                    {
                        foreach (var selectedRecipe in selectedRecipes)
                        {
                            selectedRecipe.Items = selectedRecipe.Items.Where(i => i.Status.ToString().ToLower() == command.OptionalCommandPairs[0].Value.ToLower()).ToList();
                        }
                    }
                }
                return selectedRecipes.Print();
            }
        }

        private string ShowAllUserRecipes()
        {
            var userRecipes = _userService.GetUserRecipes();
            return userRecipes.Print();
        }

        private bool IsUserLogin()
        {
            return string.IsNullOrEmpty(_currentUser) == false;
        }

        private Command PreprocessCommand(string rawCommand)
        {
            var command = new Command();
            var parts = CommandLineHelper.SplitCommandLine(rawCommand).ToList();

            if (!parts.Any()) return command;

            var rawCommandType = parts[0].ToLower();

            if (rawCommandType == "help") command.CommandType = CommandType.Help;
            else if (rawCommandType == "add") command.CommandType = CommandType.Add;
            else if (rawCommandType == "edit") command.CommandType = CommandType.Edit;
            else if (rawCommandType == "help") command.CommandType = CommandType.Help;
            else if (rawCommandType == "login") command.CommandType = CommandType.Login;
            else if (rawCommandType == "show" || rawCommandType == "list") command.CommandType = CommandType.Show;
            else if (rawCommandType == "remove" || rawCommandType == "delete") command.CommandType = CommandType.Remove;
            else if (rawCommandType == "publish") command.CommandType = CommandType.Publish;
            else if (rawCommandType == "takedown") command.CommandType = CommandType.Takedown;
            else if (rawCommandType == "purchase" || rawCommandType == "buy") command.CommandType = CommandType.Purchase;
            else if (rawCommandType == "review") command.CommandType = CommandType.Review;

            if (parts.Count > 1)
            {
                var mainObjKey = parts[1].ToLower();
                DomainObjectType mainObjKeyType = DomainObjectType.Unknown;

                if (mainObjKey == "user") mainObjKeyType = DomainObjectType.User;
                else if (mainObjKey == "recipe" || mainObjKey == "privaterecipe") mainObjKeyType = DomainObjectType.PrivateRecipe;
                else if (mainObjKey == "market" || mainObjKey == "publicrecipe") mainObjKeyType = DomainObjectType.PublicRecipe;
                else if (mainObjKey == "item" || mainObjKey == "recipeitem") mainObjKeyType = DomainObjectType.RecipeItem;
                else if (mainObjKey == "balance" || mainObjKey == "account") mainObjKeyType = DomainObjectType.UserAccountBalance;
                else if (mainObjKey == "system" || mainObjKey == "systemstatus") mainObjKeyType = DomainObjectType.SystemStatus;

                var mainObjValue = string.Empty;
                if (parts.Count > 2)
                {
                    mainObjValue = parts[2];
                }

                command.MainObjPair = new KeyValuePair<DomainObjectType, string>(mainObjKeyType, mainObjValue);
            }

            for (int i = 3; i < parts.Count; i = i + 2)
            {
                var key = parts[i];
                var value = string.Empty;
                if (parts.Count > i + 1)
                {
                    value = parts[i + 1];
                }

                //if(command.OptionalCommandPairs == null) command.OptionalCommandPairs = new List<KeyValuePair<string, string>>();
                command.OptionalCommandPairs.Add(new KeyValuePair<string, string>(key, value));
            }

            return command;
        }

        private string GetCommandHelp()
        {
            return @"System Commands :
* add user <username>
* login user <username>

* show recipe
* show recipe <id or fuzzy name>
* show recipe <id or fuzzy name> -status <value>
* add recipe <title>
* edit recipe <id or fuzzy name> -title <new title>
* remove recipe <id>

* add item <item description> -recipe <recipe id> 
* edit item <id> -recipe <recipe id> -description/desc <new description>
* edit item <id> -recipe <recipe id> -status <finish or reset>
* remove item <id> -recipe <recipe id>

* publish recipe <private recipe id> -price <value>
* takedown recipe <public recipe id>

* show market/publicRecipe
* purchase publicRecipe <public recipe id>
* review publicRecipe <public recipe id> -rating <value> -comment <value>

* show balance/account
* show system/systemstatus";
        }
    }
}
