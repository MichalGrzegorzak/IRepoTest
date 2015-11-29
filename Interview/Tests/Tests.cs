using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System;
using Interview.Implemenation;
using NUnit.Framework;

namespace Interview
{
    [TestFixture]
    public class Tests
    {
        //private List<IStoreable> _storedData;
        private IDataContext _context;
        private const int _notExisitingId = -111;

        private static readonly object[] DifferentTypesNewIds =
        {
            new object[] { new User() {Id = 6, Name = "D"} },
            new object[] { new Car() {Id = 7, ProductionYear = 1950 } },
        };
        private static readonly object[] DifferentTypesExistingIds =
        {
            new object[] { new User() {Id = 3, Name = "EEE"} },
            new object[] { new Car() {Id = 2, ProductionYear = 2222 } },
        };
            
        [SetUp]
        public void InitializeTest()
        {
            var storedData = new List<IStoreable>();
            storedData.Add(new Car() { Id = 1, ProductionYear = 1900 });
            storedData.Add(new Car() { Id = 2, ProductionYear = 2000 });
            storedData.Add(new User() { Id = 3, Name = "A" });
            storedData.Add(new User() { Id = 4, Name = "B" });
            storedData.Add(new User() { Id = 5, Name = "C" });

            _context = new DataContext(storedData);
        }

        [Test]
        public void All_Cars_returns_only_all_cars_types()
        {
            //arrange
            var carsRepository = new InMemoryRepo<Car>(_context);

            //act
            var resultData = carsRepository.All();

            //assert
            Assert.That(resultData.Count(), Is.EqualTo(2));
        }

        [Test]
        public void All_Users_returns_only_all_users_types()
        {
            //arrange
            var userRepository = new InMemoryRepo<User>(_context);

            //act
            var resultData = userRepository.All();

            //assert
            Assert.That(resultData.Count(), Is.EqualTo(3));
        }

        [Test]
        public void All_by_IStorable_returns_all_items()
        {
            //arrange
            var repository = new InMemoryRepo<IStoreable>(_context);

            //act
            var resultData = repository.All();

            //assert
            Assert.That(resultData.Count(), Is.EqualTo(5));
        }

        [Test, TestCaseSource("DifferentTypesExistingIds")]
        public void Delete_existing_entity_should_remove_it<T>(T item) where T : IStoreable
        {
            //arrange
            var repository = CreateRepo(item, _context);
            var deleteElementId = _context.Entities.Values.OfType<T>().Last().Id;
            int countBefore = _context.Entities.Values.OfType<T>().Count();
            Console.WriteLine("Removing Id: " + deleteElementId);

            //act
            repository.Delete(deleteElementId);

            //asert
            int countAfter = _context.Entities.Values.OfType<T>().Count();
            Assert.That(countAfter, Is.EqualTo(--countBefore));
            Assert.IsNull(_context.Entities.Keys.FirstOrDefault(x => x.Equals(deleteElementId)));
        }

        [Test, TestCaseSource("DifferentTypesNewIds")]
        public void Save_new_entity_should_add_new_element<T>(T newElement) where T : IStoreable
        {
            //arrange
            var repository = new InMemoryRepo<IStoreable>(_context);
            int countBefore = _context.Entities.Count();

            //act
            repository.Save(newElement);

            //assert
            int countAfter = _context.Entities.Count();
            Assert.That(countAfter, Is.EqualTo(++countBefore));
            Assert.IsNotNull(_context.Entities.FirstOrDefault(x => x.Key.Equals(newElement.Id)));
        }

        [Test]
        public void Save_existing_entity_should_update_previous_one()
        {
            //arrange
            var userRepository = new InMemoryRepo<User>(_context);
            var newElementButSameId = new User() { Id = 3, Name = "AAA" };

            //act
            userRepository.Save(newElementButSameId);

            //assert
            Assert.AreEqual("AAA", (_context.Entities[3] as User).Name);
        }

        [Test]
        public void FindById_that_exists_should_return_element()
        {
            //arrange
            var repository = new InMemoryRepo<User>(_context);

            //act
            var result = repository.FindById(4);

            //assert
            Assert.AreEqual(4, result.Id);
        }

        [Test]
        public void FindById_that_exists_but_type_is_diffeent_should_return_null()
        {
            //arrange
            var idOfCar = ((Car)_context.Entities[1]).Id;
            var userRepository = new InMemoryRepo<User>(_context);

            //act
            var result = userRepository.FindById(idOfCar);

            //assert
            Assert.IsNull(result);
        }

        [Test]
        public void FindById_that_not_exists_should_return_null()
        {
            //arrange
            var repository = new InMemoryRepo<User>(_context);

            //act
            var result = repository.FindById(_notExisitingId);

            //assert
            Assert.IsNull(result);
        }

        [Test]
        public void Delete_not_existing_element_should_throw_exception()
        {
            //arrange
            var repository = new InMemoryRepo<User>(_context);

            Assert.Throws<InvalidOperationException>(() =>
                repository.Delete(_notExisitingId)
            );
        }

        [Test]
        public void FindById_throws_exception_if_id_is_null()
        {
            var repository = new InMemoryRepo<User>(_context);

            Assert.Throws<ArgumentNullException>(() =>
                repository.FindById(null)
            );
        }
        [Test]
        public void Save_throws_exception_if_item_is_null()
        {
            var repository = new InMemoryRepo<User>(_context);

            Assert.Throws<ArgumentNullException>(() =>
                repository.Save(null)
            );
        }
        [Test]
        public void Delete_throws_exception_if_id_is_null()
        {
            var repository = new InMemoryRepo<User>(_context);

            Assert.Throws<ArgumentNullException>(() =>
                repository.Delete(null)
            );
        }

        /// <summary>
        /// Create generic repository at runtime
        /// </summary>
        private static InMemoryRepo<T> CreateRepo<T>(T type, IDataContext ctx) where T : IStoreable
        {
            return (InMemoryRepo<T>)Activator.CreateInstance(typeof(InMemoryRepo<T>), new object[] { ctx });
        }
    }
}