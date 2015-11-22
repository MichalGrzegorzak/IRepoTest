using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System;
using Interview.Implemenation;
using Moq;
using NUnit.Framework;

namespace Interview
{
    [TestFixture]
    public class TestInMemoryRepoWithMoq
    {
        private IRepository<User> repository;
        private Mock<IDataContext> mockedDataContext;
        private List<IStoreable> users;

        [SetUp]
        public void Initialize()
        {
            users = new List<IStoreable>(new[]
            {
                new User() {Id = 1, Name = "A"},
                new User() {Id = 2, Name = "B"},
                new User() {Id = 3, Name = "C"},
                new User() {Id = 4, Name = "D"},
            });

            mockedDataContext = new Mock<IDataContext>();
            repository = new InMemoryRepo<User>(mockedDataContext.Object);
        }
        
        [Test]
        public void Get_All_entities_should_return_all_entities()
        {
            //arrange
            mockedDataContext.Setup(x => x.Entities).Returns(users);

            //act
            var resultData = repository.All();

            //assert
            Assert.IsTrue(users.SequenceEqual(resultData));
        }

        [Test]
        public void Delete_existing_entity_should_remove_it()
        {
            //arrange
            mockedDataContext.Setup(x => x.Entities).Returns(users);
            mockedDataContext.SetupSet(x => x.Entities = It.IsAny<List<IStoreable>>())
                .Callback<List<IStoreable>>(x => users = x);
            var deleteElementId = users[2].Id;

            //act
            repository.Delete(deleteElementId);

            //assert
            Assert.IsNull(users.FirstOrDefault(x => x.Id.Equals(deleteElementId)));
        }

        [Test]
        public void Save_new_entity_should_pass()
        {
            //arrange
            mockedDataContext.SetupGet(x => x.Entities).Returns(users);
            mockedDataContext.SetupSet(x => x.Entities = It.IsAny<List<IStoreable>>())
                .Callback<List<IStoreable>>(x => users = x);

            var newElement = new User() {Id = 5, Name = "E"};

            //act
            repository.Save(newElement);

            //assert
            Assert.IsNotNull(users.FirstOrDefault(x => x.Id.Equals(newElement.Id)));
        }
        
        [Test]
        public void Save_existing_entity_should_update_previous_one()
        {
            //arrange
            mockedDataContext.Setup(x => x.Entities).Returns(users);
            mockedDataContext.SetupSet(x => x.Entities = It.IsAny<List<IStoreable>>())
                .Callback<List<IStoreable>>(x => users = x);
            string previousName = ((User)users[0]).Name;
            var newElementButSameId = new User() { Id = 1, Name = "AAA" };
            
            //act
            repository.Save(newElementButSameId);

            //assert
            Assert.AreNotEqual(newElementButSameId.Name, previousName);
        }

        [Test]
        public void FindById_that_exists_should_return_element()
        {
            //arrange
            mockedDataContext.Setup(x => x.Entities).Returns(users);

            //act
            var result = repository.FindById(users[0].Id);

            //assert
            Assert.IsNotNull(result);
        }

        [Test] 
        public void FindById_that_not_exists_should_return_null()
        {
            //arrange
            mockedDataContext.Setup(x => x.Entities).Returns(users);

            //act
            var result = repository.FindById(-111);

            //assert
            Assert.IsNull(result);
        }

        [Test]
        public void Delete_not_existing_element_should_throw_exception()
        {
            //arrange
            mockedDataContext.Setup(x => x.Entities).Returns(users);

            Assert.Throws<InvalidOperationException>(() =>
                repository.Delete(-111)
            );
        }

        [Test]
        public void FindById_if_id_null_then_throw_exception()
        {
            //arrange
            mockedDataContext.Setup(x => x.Entities).Returns(users);

            Assert.Throws<ArgumentNullException>(() =>
                repository.FindById(null)
            );
        }
        [Test]
        public void Save_if_item_null_then_throw_exception()
        {
            //arrange
            mockedDataContext.Setup(x => x.Entities).Returns(users);

            Assert.Throws<ArgumentNullException>(() =>
                repository.Save(null)
            );
        }
        [Test]
        public void Delete_if_id_null_then_throw_exception()
        {
            //arrange
            mockedDataContext.Setup(x => x.Entities).Returns(users);

            Assert.Throws<ArgumentNullException>(() =>
                repository.Delete(null)
            );
        }
    }
}