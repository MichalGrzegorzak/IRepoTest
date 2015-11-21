using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System;
using Interview.Implemenation;
using NUnit.Framework;

namespace Interview
{
    /// <summary>
    /// Derived from InMemoryRepo, to open access to private property - 'entities'
    /// I could use Moq here, but You might think that this as 'hack'
    /// </summary>
    public class InMemoryRepoOpenForTests<T> : InMemoryRepo<T> where T : IStoreable
    {
        public InMemoryRepoOpenForTests()
            : this(null)
        {
        }
        public InMemoryRepoOpenForTests(IList<T> entities)
            : base(entities)
        {
        }

        public IList<T> Entities { get { return this.entities; } }
    }

    [TestFixture]
    public class TestInMemoryRepo
    {
        private IRepository<User> repository = null;

        public IList<User> SetupRepositoryWithUsers(IList<User> users = null)
        {
            users = users ?? new User[]
            {
                new User() {Id = 1, Name = "A"},
                new User() {Id = 2, Name = "B"},
                new User() {Id = 3, Name = "C"},
                new User() {Id = 4, Name = "D"},
            };
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
            Assert.That(resultData.ToList().GetHashCode(), Is.EqualTo(inputData.ToList().GetHashCode()));
        }

        [Test]
        public void Get_All_entities_should_not_return_null_even_if_no_elements()
        {
            IList<User> inputData = null;
            repository = new InMemoryRepo<User>(inputData);
            
            var resultData = repository.All();

            Assert.That(resultData, Is.Not.Null);
        }
        
        [Test]
        public void Delete_existing_entity_should_remove_it()
        {
            var inputData = SetupRepositoryWithUsers(null);
            var deleteElement = inputData[2];

            repository.Delete(deleteElement.Id);

            Assert.That(resultData, Is.Not.Null);
        }

        [Test]
        public void Delete_not_existing_element_should_throw_exception()
        {

        }

        [Test]
        public void Save_new_entity_should_pass()
        {

        }
        
        [Test]
        public void Save_existing_entity_should_update_previous_one()
        {
        }

        [Test]
        public void FindById_that_exists_should_return_element()
        {
        }

        [Test] 
        public void FindById_that_not_exists_should_return_null()
        {
        }

        [Test]
        public void FindById_if_id_null_then_throw_exception()
        {
        }
        [Test]
        public void Save_if_item_null_then_throw_exception()
        {
        }
        [Test]
        public void Delete_if_id_null_then_throw_exception()
        {
        }
    }
}