

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography.X509Certificates;

var teams = new Dictionary<int, (string Team1, string Team2, int scoresT1, int scoresT2, string[] marksman)>()
    {
        {1, ("Belgium", "Canada", 0, 0, Array.Empty<string>()) },
        {2, ("Morocco", "Croatia", 0, 0, Array.Empty<string>()) },
        {3, ("Belgium", "Morocco", 0, 0, Array.Empty<string>()) },
        {4, ("Croatia", "Canada", 0, 0, Array.Empty<string>()) },
        {5, ("Croatia", "Belgium", 0, 0, Array.Empty<string>()) },
        {6, ("Canada", "Morocco", 0, 0, Array.Empty<string>()) },
    };

var player = new Dictionary<string, (string Position, int Rating)> ()
{
    {"Luka Modric", ("FW",  88 ) },
    {"Marcelo Brozovic", ("DF", 86 ) },
    {"Mateo Kovacic", ("MF", 84 ) },
    {"Ivan Perišić", ("FW", 84 ) },
    {"Andrej Kramaric", ("GK", 82 ) }, 
    {"Ivan Rakitić", ("DF", 82 ) },
    {"Joško Gvardiol", ("MF", 81 ) },
    {"Mario Pašalić", ("MF", 81 ) },
    {"Lovro Majer", ("FW", 80 ) },
    {"Dominik Livakovic", ("FW", 80 ) },
    {"Ante Rebić", ("GK", 80 ) }, 
    {"Josip Brekalo", ("MF", 79 ) },
    {"Borna Sosa", ("GK", 78 ) }, 
    {"Nikola Vlašić", ("DF", 78 ) },
    {"Duje Caleta-Car", ("FW", 78 ) },
    {"Dejan Lovren", ("MF", 78 ) },
    {"Mislav Oršić", ("DF", 77 ) },
    {"Marko Livaja", ("MF", 77 ) },
    {"Domagoj Vida", ("FW", 76 ) },
    {"Ante Budimir", ("DF", 76 ) },


};



StartMenu(player);

void StartMenu(Dictionary<string, (string Position, int Rating)> player)
{

    bool userInputZero = false;
    var croTeam = new Dictionary<string, (string Position, double Rating)>();

    while (userInputZero != true)
    {
        Console.WriteLine($"******************** <START MENU> ************************");
        Console.WriteLine($"Select an operation:\n" +
            $"1) Do the training\n" +
            $"2) Play a match\n" +
            $"3) Statistics\n" +
            $"4) Player control\n" +
            $"0) Exit app\n");
        var userOperationChoice = int.Parse(Console.ReadLine());

        switch (userOperationChoice)
        {
            case 0: userInputZero = true; break;
            case 1:
                player = DoTheTraining(player);
                break;

            case 2: // TASK 2 NOT FINISHED - RUN OUT OF TIME
                croTeam = AddTeamMembers(player, croTeam);
                //Team Count Verification
                if (croTeam.Count < 11)
                {
                    Console.WriteLine($"Not enough team players. Playing a match is not possible!");
                    break;
                }
                else 
                { 
                    //for(int i =1; i<7; i++)
                    //{
                    //    teams = PlayTheMatch(i,teams, croTeam);
                    //}

                }
               
                break;

            case 3:
                Statistics(teams, player, croTeam);
                break;

            case 4: PlayerControl(teams, player, croTeam);
                break;

            default:
                Console.WriteLine($"Invalid operation entered! Enter again.\nTo exit the app enter (zero)\n");
                break;
        }
    }
    Console.WriteLine("Exited app.");

}

Dictionary<string, (string P, int R)> DoTheTraining(Dictionary<string, (string Position, int Rating)> player)
{
    Console.WriteLine($"\n    Operation 1) Do the training selected.\n" +
        $"________________________________________________");

    var changedRating = new Dictionary<string, (string P, int R)>();
    Random rnd = new Random();
    int newRating = 0;
   
  
    //Change player rating (+/- 0-5%) by Random
    foreach (var item in player)
    {
        int rndRating = rnd.Next(0, 5);
        int gainOrLose = rnd.Next(0, 3);

        if (gainOrLose == 1)
        {
            newRating = item.Value.Rating - ((item.Value.Rating * rndRating) / 100);
            changedRating.Add(item.Key, (item.Value.Position, newRating));
            Console.WriteLine($"- {item.Key}, {item.Value.Position} - {rndRating}%\n Previous rating: {item.Value.Rating}\tCurrent rating: {newRating}");
        }
        else if (gainOrLose == 2)
        {
            newRating = item.Value.Rating + ((item.Value.Rating * rndRating) / 100);
            changedRating.Add(item.Key, (item.Value.Position, newRating));
        Console.WriteLine($"- {item.Key}, {item.Value.Position} + {rndRating}%\n Previous rating: {item.Value.Rating}\tCurrent rating: {newRating}");
        }
        else
        {
            newRating = item.Value.Rating;
            changedRating.Add(item.Key, (item.Value.Position, newRating));
            Console.WriteLine($"- {item.Key}\n No rating change.\t Current rating: {newRating}");
        }
        }
    return changedRating;
    }

Dictionary<string, (string Position, double Rating)> AddTeamMembers(Dictionary<string, (string Position, int Rating)> player, Dictionary<string, (string Position, double Rating)> croTeam)
{
    int maxRatingGK = 0, maxRatingDF = 0, maxRatingMF = 0, maxRatingFW = 0;
    int gkCount = 0, dfCount = 0, mfCount = 0, fwCount = 0;

    //Add players in Team
    foreach (var item in player)
    {
        //Find max Rating for each position
        if (item.Value.Position == "GK")
            maxRatingGK = (maxRatingGK < item.Value.Rating) ? item.Value.Rating : maxRatingGK;
        else if (item.Value.Position == "DF")
            maxRatingDF = (maxRatingDF < item.Value.Rating) ? item.Value.Rating : maxRatingDF;
        else if (item.Value.Position == "MF")
            maxRatingMF = (maxRatingMF < item.Value.Rating) ? item.Value.Rating : maxRatingMF;
        else
            maxRatingFW = (maxRatingFW < item.Value.Rating) ? item.Value.Rating : maxRatingFW;

        //Add players in Team, selected by position & max Rating
        if (item.Value.Position == "GK" && item.Value.Rating == maxRatingGK && gkCount != 1)
        {
            croTeam.Add(item.Key, (item.Value.Position, item.Value.Rating));
            gkCount++;
        }
        else if (item.Value.Position == "DF" && item.Value.Rating == maxRatingDF && dfCount != 4)
        {
            croTeam.Add(item.Key, (item.Value.Position, item.Value.Rating));
            maxRatingDF = 0;
            dfCount++;
        }
        else if (item.Value.Position == "MF" && item.Value.Rating == maxRatingMF && mfCount != 3)
        {
            croTeam.Add(item.Key, (item.Value.Position, item.Value.Rating));
            maxRatingMF = 0;
            mfCount++;
        }
        else if (item.Value.Position == "FW" && item.Value.Rating == maxRatingFW && fwCount != 3)
        {
            croTeam.Add(item.Key, (item.Value.Position, item.Value.Rating));
            maxRatingFW = 0;
            fwCount++;
        }

    }

    //Print Team players
    foreach (var item in croTeam)
        Console.WriteLine($" {item.Key}, {item.Value.Position}, {item.Value.Rating}");

    return croTeam;
}


void Statistics(Dictionary<int, (string Team1, string Team2, int scoresT1, int scoresT2, string[] marksman)> teams, Dictionary<string, (string Position, int Rating)> player, Dictionary<string, (string Position, double Rating)> croTeam)
{
    bool userInputZero = false;

    while (userInputZero != true)
        userInputZero = StatisticsMenu(teams, player, croTeam);
}


bool StatisticsMenu(Dictionary<int, (string Team1, string Team2, int scoresT1, int scoresT2, string[] marksman)> teams, Dictionary<string, (string Position, int Rating)> player, Dictionary<string, (string Position, double Rating)> croTeam)
{

    Console.WriteLine($"******************** <STATISTICS MENU> ********************");
    Console.WriteLine($"Select an operation:\n" +
        $"1) Print by stored order\n" +
        $"2) Print ascendingly by rating\n" +
        $"3) Print descendingly by rating\n" +
        $"4) Print alphabetically\n" +
        $"5) Print player/-s by requested rating\n" +
        $"6) Print player/-s by requested position\n" +
        $"7) Print current 11 players\n" +
        $"8) Print strikers & theirs strike numbers\n" +
        $"9) Print all team results\n" +
        $"10) Print final results for all teams\n" +
        $"11) Print score table\n" +
        $"0) Return to START MENU\n");

    var userOperationChoice = int.Parse(Console.ReadLine());

    switch (userOperationChoice)
    {
        case 0:
            Console.WriteLine("Returning back to the Start Menu.\n");
            return true;
        case 1:
            Console.WriteLine("Printing all players.\n");
            foreach (var item in player)
                Console.WriteLine($" {item.Key}, {item.Value.Position}, {item.Value.Rating}");

            break;
        case 2:
            Console.WriteLine($"Printing all players ascendingly by rating.");
            var asc = player
                   .OrderBy(p => p.Value.Rating)
                   .ToDictionary(p => p.Key, p => p.Value);

            foreach (var item in asc)
                Console.WriteLine($" {item.Key}, {item.Value.Position}, {item.Value.Rating}");
            break;

        case 3:
            Console.WriteLine($"Printing all players descendingly by rating");
            var desc = player
                .OrderByDescending(p => p.Value.Rating)
                .ToDictionary(p => p.Key, p => p.Value);

            foreach (var item in desc)
                Console.WriteLine($" {item.Key}, {item.Value.Position}, {item.Value.Rating}");
            break;

        case 4:
            Console.WriteLine($"Printing all players alphabetically");
            var alph = (
                from teamMate in player
                orderby teamMate.Key
                select teamMate
            ).ToList();

            foreach (var item in alph)
                Console.WriteLine($" {item.Key}, {item.Value.Position}, {item.Value.Rating}");
            break;

        case 5:
            Console.WriteLine($"5) Print player/-s by requested rating");
            StatisticsOpFive(player);
            break;

        case 6:
            Console.WriteLine($"6) Print player/-s by requested position");
            StatisticsOpSix(player);
            break;

        case 7:
            Console.WriteLine("7) Print current 11 players");
            if (croTeam.Count > 0)
            {
                foreach (var item in croTeam)
                    Console.WriteLine($" {item.Key}, {item.Value.Position}, {item.Value.Rating}");
            }
            else Console.WriteLine("Assemble the team first by selectiong option 2) in START MENU");
            break;

        case 8: // RETURN HERE AFTER FINISHING TASK 2!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            break;

        case 9: // RETURN HERE AFTER FINISHING TASK 2!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            break;

        case 10:// RETURN HERE AFTER FINISHING TASK 2!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            break;

        case 11:// RETURN HERE AFTER FINISHING TASK 2!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            break;

        default:
            Console.WriteLine($"Invalid operation entered! Returning to the START MENU.\n");
            return true;
    }
    Console.WriteLine($"\n0) Return to START MENU\n" +
        $"1) Repeat the STATISTICS MENU\n");
    userOperationChoice = int.Parse(Console.ReadLine());

    while (userOperationChoice != 0 && userOperationChoice != 1)
    {
        Console.WriteLine("Invalid operation entered! Enter again.\t");
        userOperationChoice = int.Parse(Console.ReadLine());
    }
        if (userOperationChoice == 0)
        return true;
    else if (userOperationChoice == 1)
        return false;

    return false;
}

void StatisticsOpFive(Dictionary<string, (string Position, int Rating)> player)
{
    Console.WriteLine("Enter the rating to search the player/-s by:\t");
    var inputRating = int.Parse(Console.ReadLine());

    foreach (var item in player)
        if (item.Value.Rating == inputRating)
            Console.WriteLine($"{item.Key} {item.Value.Position} {item.Value.Rating}\n");
}

void StatisticsOpSix(Dictionary<string, (string Position, int Rating)> player)
{
    Console.WriteLine("Enter the position to search the player/-s by:\t");
    var inputPosition = Console.ReadLine();

    foreach (var item in player)
        if (string.Compare(item.Value.Position, inputPosition, true) == 0)
            Console.WriteLine($"{item.Key} {item.Value.Position} {item.Value.Rating}");
}


void PlayerControl(Dictionary<int, (string Team1, string Team2, int scoresT1, int scoresT2, string[] marksman)> teams, Dictionary<string, (string Position, int Rating)> player, Dictionary<string, (string Position, double Rating)> croTeam)
{
    bool userInputZero = false;

    while (userInputZero != true)
        userInputZero = PlayerControlMenu(teams, player, croTeam);
}


bool PlayerControlMenu(Dictionary<int, (string Team1, string Team2, int scoresT1, int scoresT2, string[] marksman)> teams, Dictionary<string, (string Position, int Rating)> player, Dictionary<string, (string Position, double Rating)> croTeam)
{
    Console.WriteLine($"*************** <PLAYER CONTROL MENU> ***************");
    Console.WriteLine($"Select an operation:\n" +
        $"1) New player input\n" +
        $"2) Delete player\n" +
        $"3) Edit player\n" +
        $"0) Return to START MENU\n");

    var userOperationChoice = int.Parse(Console.ReadLine());

    switch (userOperationChoice)
    {
        case 0:
            Console.WriteLine("Returning back to the Start Menu.\n");
            return true;

        case 1:
            Console.WriteLine("1) New player input");
            if (NewPlayerInput(player) == true)
            {
                foreach (var item in player)
                    Console.WriteLine($" {item.Key}, {item.Value.Position}, {item.Value.Rating}");
            }
            break;

        case 2:
            Console.WriteLine("2) Delete player");
            Console.WriteLine("Enter the name & surname of the player to be deleted:\n");
            string playerToBeDeleted = Console.ReadLine();
            
            if (DeletePlayer(player, playerToBeDeleted) != false)
            {
                foreach (var item in player) Console.WriteLine($"{item.Key} {item.Value.Position} {item.Value.Rating}");
            }
    
            break;

        case 3:
            bool userInputZero = false;

            while (userInputZero != true)
                userInputZero = EditPlayerMenu(player, croTeam);
            return true;



        default:
            Console.WriteLine($"Invalid operation entered! Returning to the START MENU.\n");
            return true;
    }
    Console.WriteLine($"\n0) Return to START MENU\n" +
        $"1) Return to STATISTICS MENU\n" +
        $"2) Repeat the PLAYER CONTROL MENU\n");
    userOperationChoice = int.Parse(Console.ReadLine());

    while (userOperationChoice != 0 && userOperationChoice != 1 && userOperationChoice != 2)
    {
        Console.WriteLine("Invalid operation entered! Enter again.\t");
        userOperationChoice = int.Parse(Console.ReadLine());
    }
    if (userOperationChoice == 0)
        return true;
    else if (userOperationChoice == 1)
        Statistics(teams, player, croTeam);
    else return false;

    return false;
}








bool NewPlayerInput(Dictionary<string, (string Position, int Rating)> player)
{
    bool teamNumFull = false, printPlayers = true;
    while (teamNumFull == false)
    {
        if (player.Count < 26)
        {
            //User input new player name & check accuracy
            Console.WriteLine("Enter the name of a new player: \t");
            var newName = Console.ReadLine();
            newName = NewName(player, newName);

            if (newName != null)
            {

                //User input new player position & check accuracy
                Console.WriteLine("Enter the position of a new player: \t");
                var newPosition = Console.ReadLine();
                var positions = new List<string>() { "GK", "DF", "MF", "FW" };
                newPosition = NewPosition(newPosition, positions);

                if (newPosition != null)
                {
                    //User input new player rating & check accuracy
                    Console.WriteLine("Enter the rating of a new player: \t");
                    int newRating = 0;
                    bool success = false;
                    success = int.TryParse(Console.ReadLine(), out newRating);
                    //bool success = int.TryParse(value, out number); true if s was successfully parsed; otherwise, false.
                    success = NewRating(success, newRating);

                    if (success == true)
                    {
                        player.Add(newName, (Position: newPosition, Rating: newRating));

                        Console.WriteLine("Add another player?");
                        var another = Console.ReadLine();
                        teamNumFull = (string.Compare(another, "Yes", true) == 0) ? false : true;
                    }
                }
            }
            else
            {
                printPlayers = false;
                teamNumFull = true;
            }
        }
        else
        {
            Console.WriteLine("Maximum number of team players (26) reached!");
            teamNumFull = true;
            printPlayers = true;
        }
    }
    return (printPlayers == false) ? false : true;
}

string NewName (Dictionary<string, (string, int)> player, string newName)
{
    if (player.ContainsKey(newName))
    {
        Console.WriteLine("Player by that name already exists!\n");
        newName = "";
        return null;
    }
    else if (newName.Trim() == "") { Console.WriteLine("Invalid input\n"); return null; }

    return newName;
}

string NewPosition(string newPosition, List<string> positions)
{
    if (!positions.Contains(newPosition.ToUpper()))
    {
        Console.WriteLine("Unknown position input!\n");
        newPosition = "";
        return null;
    }
    else if (newPosition == "") { Console.WriteLine("Invalid input!\n"); return null; }
    
    newPosition.ToUpper();
    return newPosition;
}

bool NewRating(bool success, int newRating)
{
    if (!success)
            {
        Console.WriteLine("Invalid input! Rating must be a number.\n");
        return false;
    }else if (newRating < 1 || newRating > 100)
    {
        Console.WriteLine("Invalid input! Rating must be a number between 1 and 100.");
        return false;
    }

    return true;
}

bool DeletePlayer(Dictionary<string, (string, int)> player, string deleteMe)
{
    if (player.ContainsKey(deleteMe)) { 
    player.Remove(deleteMe);
        Console.WriteLine($"The player {deleteMe} is deleted.\n");
        return true;
    }
    else Console.WriteLine($"The player you are trying to delete ({deleteMe}) doesn't exist.\n");
    return false; 
}

bool EditPlayerMenu(Dictionary<string, (string, int)> player, Dictionary<string, (string Position, double Rating)> croTeam)
{
    Console.WriteLine($"3) ~~~~~~~~~~~~~~~~~~ EDIT PLAYER MENU ~~~~~~~~~~~~~~~~~~");
    Console.WriteLine($"Select an operation:\n" +
            $"1) Edit player name & surname\n" +
            $"2) Change player position\n" +
            $"3) Change player rating\n" +
            $"0) Return to START MENU\n");

    var userOpInputEditMenu = int.Parse(Console.ReadLine());
    bool loop = true;

    switch (userOpInputEditMenu)
    {
        case 0: StartMenu(player);
            return true;

        case 1:
            Console.WriteLine("1) Edit player name & surname\n");
            while(loop == true)
            loop = EditPlayerNameSurname(player);

            break;

        case 2:
            Console.WriteLine("2) Change player position\n");
            Console.WriteLine("Enter the name of a player you want to edit:\t");
            var repositionMe = Console.ReadLine();
           
            Console.WriteLine("\nEnter new position (GK, MF, DF, FW)\n");
            var newPosition = Console.ReadLine().ToUpper();
            if (!new string[4] { "GK", "DF", "MF", "FW" }.Contains(newPosition))
                Console.WriteLine("\nInvalid input!\n");
            
            while (loop == true)
                loop = ChangePlayerPosition(player, repositionMe, newPosition);
            break;

        case 3:
            Console.WriteLine("3) Change player rating");
            Console.WriteLine("Enter the name of a player you want to edit:\t");
            var rerateMe = Console.ReadLine();

            while (loop == true)
            {
                Console.WriteLine("Enter new rating:\n");
                var rating = int.Parse(Console.ReadLine());
                if (rating < 1 || rating > 100)
                {
                    Console.WriteLine("Rating must be a number between 1-100");
                    loop = true;
                }
                else
                    loop = ChangeplayerRating(player, rerateMe, rating);
            }
            break;

        default:
            Console.WriteLine($"Invalid operation entered! Returning to the PLAYER CONTROL MENU.\n");
            return true;
    }
    Console.WriteLine($"\n0) Return to START MENU\n" +
        $"1) Return to STATISTICS MENU\n" +
        $"2) Return to the PLAYER CONTROL MENU\n" +
        $"3) Repeat the EDIT PLAYER MENU\n");
    userOpInputEditMenu = int.Parse(Console.ReadLine());

    while (userOpInputEditMenu != 0 && userOpInputEditMenu != 1 && userOpInputEditMenu != 2 && userOpInputEditMenu != 3)
    {
        Console.WriteLine("Invalid operation entered! Enter again.\t");
        userOpInputEditMenu = int.Parse(Console.ReadLine());
    }
    if (userOpInputEditMenu == 0)
        return true;
    else if (userOpInputEditMenu == 1)
        Statistics(teams, player, croTeam);
    else if (userOpInputEditMenu == 2)
        PlayerControl(teams, player, croTeam);
    else return false;


    return false;

}

bool EditPlayerNameSurname(Dictionary<string, (string Position, int Rating)> player)
{
    Console.WriteLine("Enter the name of a player you want to edit:\t");
    var renameMe = Console.ReadLine();
    Console.WriteLine("Enter the new name:\n");
    var renamed = Console.ReadLine();

    if (player.ContainsKey(renameMe))
    {
        player.Add(renamed, (player[renameMe].Position, player[renameMe].Rating));
        player.Remove(renameMe);
        Console.WriteLine("Player name edited successfully!");
        return false;
    }
    else Console.WriteLine($"The player you are trying to edit ({renameMe}) doesn't exist.\n");
    return true;
}

bool ChangePlayerPosition(Dictionary<string, (string Position, int Rating)> player, string repositionMe, string newPosition)
{
    if (string.Compare(player[repositionMe].Position, newPosition, true) == 0)
    {
        Console.WriteLine("Player is already at that position.");
        return true;
    }
    else
        player[repositionMe] = (newPosition, player[repositionMe].Rating);

    Console.WriteLine("Player position changed successfully!");
    return false;
}

bool ChangeplayerRating(Dictionary<string, (string Position, int Rating)> player, string rerateMe, int rating)
{
    if (player[rerateMe].Rating == rating)
    {
        Console.WriteLine("Player already has that rating. Enter another rating.");
        return true;
    }
    else
        player[rerateMe] = (player[rerateMe].Position, rating);

    Console.WriteLine("Player rating changed successfully!");
    return false;



}


//Dictionary<int, (string Team1, string Team2, int scoresT1, int scoresT2, string[] marksman)> PlayTheMatch(int i, Dictionary<int, (string Team1, string Team2, int scoresT1, int scoresT2, string[] marksman)> teams, Dictionary<string, (string Position, double Rating)> croTeam)
//{
//    Random rnd = new Random();
//    int gameResultTeam1 = rnd.Next(0, 10);
//    int gameResultTeam2 = rnd.Next(0, 10);

//    var match = teams[i];
//    int j = 0;

//    string matchWinner = (gameResultTeam1 > gameResultTeam2) ? match.Team1 : match.Team2;
//    int goals = (matchWinner == match.Team1 ? gameResultTeam1 : gameResultTeam2);

//    string[] marksman = Array.Empty<string>();

//    if (match.Team1 == "Croatia" || match.Team2 == "Croatia")
//    {
//        var croGoals = (match.Team1 == "Croatia") ? gameResultTeam1 : gameResultTeam2;
//        marksman = new string[croGoals];

//        if (croGoals > 0)
//        {
//            var rnd2 = new Random();
//            int strikerNum = 0;

//            while (croGoals != 0)
//            {
//                strikerNum = rnd2.Next(1, 11); //From 1 to 11 players
//                marksman[croGoals] = croTeam;

//                croGoals--;
//            }
//        }
//    }


//    return (Team1: match.Team1, Team2: match.Team2, scoresT1: gameResultTeam1, scoresT2: gameResultTeam2, marksman: ) > ();
//}