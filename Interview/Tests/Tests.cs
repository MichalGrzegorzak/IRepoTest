using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System;
using Interview.Implemenation;
using NUnit.Framework;

namespace Interview
{
    [TestFixture]
    public class TestInMemoryRepo
    {
        private IRepository<User> repository = null;

        public IList<User> SetupRepositoryWithUsers(IList<User> users = null)
        {
            users = users ?? new List<User>(new []
            {
                new User() {Id = 1, Name = "A"},
                new User() {Id = 2, Name = "B"},
                new User() {Id = 3, Name = "C"},
                new User() {Id = 4, Name = "D"},
            });
            
            //var ctx = new DataContext();
            //ctx.Entities = users;

            //repository = new InMemoryRepo<User>(ctx);
            repository = new InMemoryRepo<User>(users);
            return users;
        }
            
        [Test]
        public void Get_All_entities_should_return_all_entities()
        {
            //arrange
            var inputData = SetupRepositoryWithUsers(null);

            //act
            var resultData = repository.All();

            //assert
            Assert.IsTrue(inputData.SequenceEqual(resultData));
        }

        [Test]
        public void Get_All_entities_should_not_return_null_even_if_no_elements()
        {
            //arrange
            repository = new InMemoryRepo<User>(null); //new DataContext()

            //act
            var resultData = repository.All();

            //assert
            Assert.That(resultData, Is.Not.Null);
        }
        
        [Test]
        public void Delete_existing_entity_should_remove_it()
        {
            //arrange
            var inputData = SetupRepositoryWithUsers(null);
            var deleteElementId = inputData[2].Id;

            //act
            repository.Delete(deleteElementId);

            //assert
            Assert.IsNull(inputData.FirstOrDefault(x => x.Id.Equals(deleteElementId)));
        }

        [Test]
        public void Delete_not_existing_element_should_throw_exception()
        {
            SetupRepositoryWithUsers(null);

            Assert.Throws<InvalidOperationException>(() => 
                repository.Delete(-111) 
            );
        }

        [Test]
        public void Save_new_entity_should_pass()
        {
            //arrange
            var inputData = SetupRepositoryWithUsers(null);
            var newElement = new User() {Id = 5, Name = "E"};

            //act
            repository.Save(newElement);

            //assert
            Assert.IsNotNull(inputData.FirstOrDefault(x => x.Id.Equals(newElement.Id) ));
        }
        
        [Test]
        public void Save_existing_entity_should_update_previous_one()
        {
            //arrange
            var inputData = SetupRepositoryWithUsers(null);

            var newElementButSameId = new User() { Id = 1, Name = "AAA" };
            var existingElement = inputData[0];
            var id = 1;

            //act
            var x1 = repository.FindById(id);
            repository.Save(newElementButSameId);
            var x2 = repository.FindById(id);

            //assert
            //existingElement
            //Assert.IsNotNull(inputData.FirstOrDefault(x => x.Name.Equals(newElement.Id)));
        }

        [Test]
        public void FindById_that_exists_should_return_element()
        {
            //arrange
            var inputData = SetupRepositoryWithUsers(null);

            var result = repository.FindById(inputData[0].Id);

            Assert.IsNotNull(result);
        }

        [Test] 
        public void FindById_that_not_exists_should_return_null()
        {
            //arrange
            var inputData = SetupRepositoryWithUsers(null);

            //act
            var result = repository.FindById(-111);

            //assert
            Assert.IsNull(result);
        }

        [Test]
        public void FindById_if_id_null_then_throw_exception()
        {
            //arrange
            var inputData = SetupRepositoryWithUsers(null);

            Assert.Throws<ArgumentNullException>(() =>
                repository.FindById(null)
            );
        }
        [Test]
        public void Save_if_item_null_then_throw_exception()
        {
            //arrange
            var inputData = SetupRepositoryWithUsers(null);

            Assert.Throws<ArgumentNullException>(() =>
                repository.Save(null)
            );
        }
        [Test]
        public void Delete_if_id_null_then_throw_exception()
        {
            //arrange
            var inputData = SetupRepositoryWithUsers(null);

            Assert.Throws<ArgumentNullException>(() =>
                repository.Delete(null)
            );
        }
    }
}