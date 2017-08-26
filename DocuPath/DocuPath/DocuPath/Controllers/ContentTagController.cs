﻿using DocuPath.DataLayer;
using DocuPath.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocuPath.Controllers
{
    public class ContentTagController : Controller
    {
        DocuPathEntities db = new DocuPathEntities();
        // GET: ContentTag
        public ActionResult Index()
        {
            return View();
        }
        //----------------------------------------------------------------------------------------------//
        #region CREATES:
        // GET: ContentTag/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContentTag/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region READS:
        // GET: ContentTag/All
        public ActionResult All()
        {
            try
            {
                ViewBag.Neurons = VERTEBRAE.GetUnhandledNeurons();
                return View(db.CONTENT_TAG.ToList());
            }
            catch (Exception x)
            {
                return RedirectToAction("Error", "Home", x.Message);
            }
        }
        // GET: ContentTag/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region UPDATES:
        // GET: ContentTag/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ContentTag/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region DELETES:
        // GET: ContentTag/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ContentTag/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region NON-CRUD ACTIONS:

        #endregion
    }
}
