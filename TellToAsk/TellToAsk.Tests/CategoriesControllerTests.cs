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

namespace TellToAsk.Tests
{
    [TestClass]
    public class CategoriesControllerTests
    {
       
          [TestMethod]
        public void  IndexMethodShouldReturnProperNumberOfCategories()
        {
            List<ApplicationUser> users = new List<ApplicationUser> { new ApplicationUser() { Id = "1asdfasdfgahasre" } };
            
            var list = new List<Category>();
            for (int i = 1; i < 6; i++)
            {
                list.Add(new Category() { CategoryId = i, Name ="name"+i, AgeRating=AgeRating.Children, Users=users});
            }
            var ctegoryRepoMock = new Mock<IRepository<Category>>();
            ctegoryRepoMock.Setup(x => x.All()).Returns(list.AsQueryable());

            var uofMock = new Mock<IUowData>();
            uofMock.Setup(x => x.Categories).Returns(() => { return ctegoryRepoMock.Object; });

            var controller = new CategoriesController(uofMock.Object);
            var viewResult = controller.Index() as ViewResult;
            var model = viewResult.Model;
            var modelAsIenum = model as IEnumerable<TellToAsk.Areas.Administration.Models.CategoryModel>;
            
            Assert.IsNotNull(viewResult, "Index action returns null.");
            Assert.IsNotNull(model, "The model is null.");
            Assert.AreEqual(5, modelAsIenum.Count());
        }

          [TestMethod]
          public void DeleteMethodShouldReturnProperCategoryModel()
          {
              var cat = new Category() { CategoryId = 10, AgeRating=AgeRating.Mature, Name="Category 1" };

              var ctegoryRepoMock = new Mock<IRepository<Category>>();
              ctegoryRepoMock.Setup(ur => ur.GetById(10)).Returns(cat);


              var uofMock = new Mock<IUowData>();
              uofMock.Setup(x => x.Categories).Returns(() => { return ctegoryRepoMock.Object; });

              var controller = new CategoriesController(uofMock.Object);
              var modelState = controller.ModelState.IsValid ;
              var viewResult = controller.Delete(10) as ViewResult;
              var model = viewResult.Model;
              var modelAsIenum = model as CategoryModel;

              Assert.IsNotNull(viewResult, "Delete action returns null.");
              Assert.IsNotNull(model, "The model is null.");
          }

    }
}
