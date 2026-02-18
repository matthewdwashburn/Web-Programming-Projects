using Gargoyles.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Gargoyles.Services
{
    public class GargoylesDatabase
    {
        private Dictionary<string, GargoyleModel> gargoyles = new();


        public GargoylesDatabase() {
            //Initialize 2 gargoyle models when the server starts
            GargoyleModel gargoyleModel1 = new GargoyleModel();
            GargoyleModel gargoyleModel2 = new GargoyleModel();
            gargoyleModel1.Name = "Entry1";
            gargoyleModel2.Name = "Entry2";
            gargoyles.Add(gargoyleModel1.Name, gargoyleModel1);
            gargoyles.Add(gargoyleModel2.Name, gargoyleModel2);
        }

        public GargoyleModel Get(string name) 
        {
            //If gargoyle model doesn't exist
            if (!gargoyles.ContainsKey(name))
            {
                return null;
            }
            //If gorgoyle model does exist
            return gargoyles[name];
        }

        public IEnumerable<GargoyleModel> GetAll()
        {
            return gargoyles.Values;
        }

        public void AddOrReplace(GargoyleModel model)
        {
            if(string.IsNullOrWhiteSpace(model.Name))
            {
                return;
            }

            model.LastUpdated = DateTime.UtcNow;
            gargoyles[model.Name] = model;
        }
    }
}
