using PopsicleFactoryCo.Models;

namespace PopsicleFactoryCo.Data
{
    // This class is an implementation of the IPopsicleInventory interface that uses a hardcoded list of popsicles to manage the inventory instead of a database and entity framework.
    // It provides methods to create, search, retrieve, update, and delete popsicle records in the inventory.
    public class PopsicleInventoryoldwithmanualdata : IPopsicleInventory
    {
        private readonly List<PopsicleModel> _listOfPopsicles = new List<PopsicleModel>();

        public PopsicleInventoryoldwithmanualdata()
        {
            _listOfPopsicles.Add(new PopsicleModel
            {
                Id = 1,
                Name = "Nicollete path hole",
                Flavour = "Chocolate",
                Price = 15,
                Quantity = 15

            });
            _listOfPopsicles.Add(new PopsicleModel
            {
                Id = 2,
                Name = "Durham style",
                Flavour = "Vanilla",
                Price = 10,
                Quantity = 10
            });
            _listOfPopsicles.Add(new PopsicleModel
            {
                Id = 3,
                Name = "Duval style",
                Flavour = "Strawberry",
                Price = 20,
                Quantity = 20
            });
            _listOfPopsicles.Add(new PopsicleModel
            {
                Id = 4,
                Name = "Deccan style",
                Flavour = "Orange",
                Price = 25,
                Quantity = 25
            });
            _listOfPopsicles.Add(new PopsicleModel
            {
                Id = 5,
                Name = "Boulevard style",
                Flavour = "Blueberry",
                Price = 20,
                Quantity = 20
            });
        }

        /// <summary>
        /// Create new popsicle and adds it to the existing inventory.
        /// </summary>
        public Task<PopsicleModel> CreatePopsicleAsync(PopsicleModel popsicle)
        {
            try
            {
                // In real time we get data from the database or any other data source.Here i have hardcoded data for simplicity.
                // Using LINQ to perform CRUD operations on a list of objects
                var existingPopsicle = _listOfPopsicles.FirstOrDefault(p => p.Id == popsicle.Id);

                // If the popsicle with the same ID already exists, throw an exception
                if (existingPopsicle != null)
                {
                    var errorMessage = $"Popsicle with ID {popsicle.Id} already exists.";
                    throw new ArgumentException(errorMessage);
                }
                else
                {
                    // Assigning the new ID to the popsicle
                    //popsicle.Id = _newPopsicleId++;
                    popsicle.Id = _listOfPopsicles.Max(p => p.Id) + 1;
                    _listOfPopsicles.Add(popsicle);
                    return Task.FromResult(popsicle);
                }
                                }
            catch (Exception ex) 
            {
                // Throws an exception if any invalid data is provided for creating a popsicle.
                // I can also log the error here using a logging framework.But leaving it for now.
                throw new ArgumentException("An error occurred while creating the popsicle.", ex);
            }            
        }

        /// <summary>
        /// Searches for popsicles in the inventory that match the specified search Item.
        /// </summary>
        public Task<IEnumerable<PopsicleModel>> SearchForPopsiclesAsync(string searchItem)
        {
            //IEnumerable<> collection loads all elements at once into memory before iteration begins.
            // It will return query which matches with the search term from the inventory list available
            var popsicles = _listOfPopsicles.Where(p => p.Name.Contains(searchItem, StringComparison.OrdinalIgnoreCase) ||
                                                       p.Flavour.Contains(searchItem, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(popsicles.AsEnumerable());
        }

        /// <summary>
        /// Get the list of all popsicles available in the inventory.
        /// </summary>
        public Task<IEnumerable<PopsicleModel>> GetListOfAllPopsicles()
        {
            return Task.FromResult(_listOfPopsicles.AsEnumerable());
        }


        /// <summary>
        /// Get popsicle by ID from the inventory.
        /// </summary>
        public Task<PopsicleModel?> GetPopsicleByIdAsync(int id)
        {
            // It will return query that matches with the ID from the inventory list available
            var popsicle = _listOfPopsicles.FirstOrDefault(p => p.Id == id);

            return Task.FromResult(popsicle);
        }

        /// <summary>
        /// Updates an existing popsicle in the inventory with new values.
        /// </summary>
        public Task<PopsicleModel?> UpdatePopsicleAsync(PopsicleModel popsicle)
        {
            //var existingPopsicle = GetPopsicleByIdAsync(popsicle.Id).Result;
            // Finding the popsicle in the inventory list that matches the provided ID
            var existingPopsicle = _listOfPopsicles.FirstOrDefault(p => p.Id == popsicle.Id);
            if (existingPopsicle != null)
            {
                // Updating the existing popsicle with new values
                existingPopsicle.Name = popsicle.Name;
                existingPopsicle.Flavour = popsicle.Flavour;
                existingPopsicle.Price = popsicle.Price;
                existingPopsicle.Quantity = popsicle.Quantity;
            }
            return Task.FromResult(existingPopsicle);
        }


        /// <summary>
        /// Deletes a popsicle from the inventory based on the provided ID.
        /// </summary>
        public Task<bool> DeletePopsicleAsync(int id)
        {
            //var popsicle = GetPopsicleByIdAsync(id).Result;
            // Finding the popsicle in the inventory list that matches the provided ID
            var popsicle = _listOfPopsicles.FirstOrDefault(p => p.Id == id);
            if (popsicle != null)
            {
                // Removing the popsicle from the inventory list
                // Using LINQ to perform CRUD operations on a list of objects along with FirstOrDefault in the above
                _listOfPopsicles.Remove(popsicle);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}

