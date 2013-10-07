using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TellToAsk.Areas.LoggedUser.Models;
using System.Web.Mvc;
using TellToAsk.Controllers;
using TellToAsk.Areas.LoggedUser.Controllers;
using Moq;
using TellToAsk.Data;
using TellToAsk.Model;
using TellToAsk.Areas.Administration.Controllers;
using TellToAsk.Areas.Administration.Models;

namespace TellToAsk.Tests
{
    [TestClass]
    public class AnswersControllerTests
    {
        [TestMethod]
   
        public void IndexMethodShouldReturnProperNumberOfAnswers()
        {
            List<ApplicationUser> users = new List<ApplicationUser> { new ApplicationUser() { Id = "1asdfasdfgahasre" } };

            var list = new List<Answer>();
            for (int i = 1; i < 6; i++)
            {
                list.Add(new Answer()
                {
                    AnswerId = i,
                    Comment = "Comment" + i,
                    User = new ApplicationUser() { Id = "1asdfasdfgahasre" },
                    DateAnswered = DateTime.Now,
                    Question = new Question() { QuestionId = i+10},
                     IsRead = true,
                });
            }
            var ansRepoMock = new Mock<IRepository<Answer>>();
            ansRepoMock.Setup(x => x.All()).Returns(list.AsQueryable());

            var uofMock = new Mock<IUowData>();
            uofMock.Setup(x => x.Answers).Returns(() => { return ansRepoMock.Object; });

            var controller = new AnswersController(uofMock.Object);
            var viewResult = controller.Index() as ViewResult;
            var model = viewResult.Model;
            var modelAsIenum = model as IEnumerable<TellToAsk.Areas.Administration.Models.AnswerModel>;

            Assert.IsNotNull(viewResult, "Index action returns null.");
            Assert.IsNotNull(model, "The model is null.");
            Assert.AreEqual(5, modelAsIenum.Count());
        }



        [TestMethod]
        public void DeleteMethodShouldReturnProperAnswerModel()
        {
            var user3 = new ApplicationUser() { Id = "3asdfasdfgahasre", UserName = "UserName3" };
            var cat = new Category() { CategoryId = 10, AgeRating=AgeRating.Mature, Name="Category 1" };
            List<ApplicationUser> users = new List<ApplicationUser> { new ApplicationUser() { Id = "1asdfasdfgahasre" } };
            var quest = new Question() { QuestionId = 10, Creator = user3, Category = cat, Title = "Q Title", Text = "Q Text", IsApproved = true, DateAsked = DateTime.Now };
            var list = new List<Answer>();
            for (int i = 1; i < 6; i++)
            {
                list.Add(new Answer()
                {
                    AnswerId = i,
                    Comment = "Comment" + i,
                    User = new ApplicationUser() { Id = "1asdfasdfgahasre", UserName = "UserName1" },
                    DateAnswered = DateTime.Now,
                    Question = quest,
                    IsRead = true, IsReported=true, IsVoted=false,
                });
            }

            var ansRepoMock = new Mock<IRepository<Answer>>();
            ansRepoMock.Setup(ur => ur.All()).Returns(list.AsQueryable());

            var uofMock = new Mock<IUowData>();
            uofMock.Setup(x => x.Answers).Returns(() => { return ansRepoMock.Object; });

            var controller = new AnswersController(uofMock.Object);
            var viewResult = controller.Delete(2) as ViewResult;
            var model = viewResult.Model;
            var modelAsAM = model as AnswerView;

            Assert.IsNotNull(modelAsAM, "Delete action returns null.");
            Assert.IsNotNull(model, "The model is null.");
        }
    }
}
