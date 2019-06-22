using System;
using System.Linq;
using System.Collections.Generic;

namespace Tavisca.Bootcamp.LanguageBasics.Exercise1
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Test(
                new[] { 3, 4 }, 
                new[] { 2, 8 }, 
                new[] { 5, 2 }, 
                new[] { "P", "p", "C", "c", "F", "f", "T", "t" }, 
                new[] { 1, 0, 1, 0, 0, 1, 1, 0 });
            Test(
                new[] { 3, 4, 1, 5 }, 
                new[] { 2, 8, 5, 1 }, 
                new[] { 5, 2, 4, 4 }, 
                new[] { "tFc", "tF", "Ftc" }, 
                new[] { 3, 2, 0 });
            Test(
                new[] { 18, 86, 76, 0, 34, 30, 95, 12, 21 }, 
                new[] { 26, 56, 3, 45, 88, 0, 10, 27, 53 }, 
                new[] { 93, 96, 13, 95, 98, 18, 59, 49, 86 }, 
                new[] { "f", "Pt", "PT", "fT", "Cp", "C", "t", "", "cCp", "ttp", "PCFt", "P", "pCt", "cP", "Pc" }, 
                new[] { 2, 6, 6, 2, 4, 4, 5, 0, 5, 5, 6, 6, 3, 5, 6 });
            Console.ReadKey(true);
        }

        private static void Test(int[] protein, int[] carbs, int[] fat, string[] dietPlans, int[] expected)
        {
            var result = SelectMeals(protein, carbs, fat, dietPlans).SequenceEqual(expected) ? "PASS" : "FAIL";
            Console.WriteLine($"Proteins = [{string.Join(", ", protein)}]");
            Console.WriteLine($"Carbs = [{string.Join(", ", carbs)}]");
            Console.WriteLine($"Fats = [{string.Join(", ", fat)}]");
            Console.WriteLine($"Diet plan = [{string.Join(", ", dietPlans)}]");
            Console.WriteLine(result);
        }

        public static List<int> getMealsByMinNutrients(List<int> mealIndices, int[] nutrient)
        {
            //Find min nutrient among the indices in mealIndices
            int minNutrient = mealIndices.Select(index => nutrient[index]).Min();
            //Find the indices of min nutrient in nutrient array
            return mealIndices.Where(index => nutrient[index] == minNutrient).ToList();
        }

        public static List<int> getMealsByMaxNutrients(List<int> mealIndices, int[] nutrient)
        {
            //Find max nutrient among the indices in mealIndices
            int maxNutrient = mealIndices.Select(index => nutrient[index]).Max();
            //Find the indices of max nutrient in nutrient array
            return mealIndices.Where(index => nutrient[index] == maxNutrient).ToList();
        }

        public static int[] getCalories(int[] protein, int[] carbs, int[] fat)
        {
            int[] calories = new int[protein.Length];
            int i;
            for(i = 0; i < protein.Length; i++)
                calories[i] = fat[i]*9 + (carbs[i] + protein[i])*5;
            return calories;
        }

        public static int[] SelectMeals(int[] protein, int[] carbs, int[] fat, string[] dietPlans)
        {
            int [] dietMeals = new int[dietPlans.Length];

            //Dictionary to access nutrients based on first character(To avoid if conditions)
            Dictionary<char, int[]> nutrientsDict = new Dictionary<char, int[]>();
            nutrientsDict.Add('P', protein);
            nutrientsDict.Add('C', carbs);
            nutrientsDict.Add('F', fat);
            nutrientsDict.Add('T', getCalories(protein, carbs, fat));

            int i;
            for(i = 0; i < dietPlans.Length; i++)
            {
                //If there is no diet plan, then return 0
                if(dietPlans[i] == "")
                {
                    dietMeals[i] = 0;
                    continue;
                }

                //List to store indices of mealPlans(Initialy all meals are included)
                List<int> mealIndices = Enumerable.Range(0, protein.Length).ToList();

                foreach(char dietItem in dietPlans[i])
                {
                    if(Char.IsUpper(dietItem))
                        mealIndices = getMealsByMaxNutrients(mealIndices, nutrientsDict[dietItem]);
                    else
                        mealIndices = getMealsByMinNutrients(mealIndices, nutrientsDict[Char.ToUpper(dietItem)]);

                    //If there is no tie, stop furthur process
                    if(mealIndices.Count == 1)
                        break;
                }
                //Diet Meal should be the lowest indexed meal
                dietMeals[i] = mealIndices[0];
            }
            return dietMeals;
        }
    }
}
