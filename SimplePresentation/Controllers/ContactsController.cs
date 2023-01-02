using AutoMapper;
using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimplePresentation.ModelViews;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePresentation.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ContactsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            //безотложно подгружаем множество связей для нашей модели представления
            //в более новых EF есть оптмизиации например .AsSplitQuery() разделяя один запрос на множество
            //чтобы избежать "декартового взрыва"
            var presentList = _context.Contacts
                .Include(x => x.Conferences)
                .Include(x => x.Phone)
                .Include(x => x.Profile)
                .AsNoTracking()
                .ToList();
            var mappedList = new List<ContactModelView>();
            foreach (var item in presentList)
            {
                mappedList.Add(_mapper.Map<ContactModelView>(item));
            }
            return View(mappedList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new ContactModelView());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ContactModelView vm)
        {
            if (ModelState.IsValid)
            {
                Contact contact = new Contact
                {
                    Phone = new Phone { PhoneNumber = vm.PhoneNumber },
                    Profile = new Domain.Profile { DisplayName = vm.DisplayName }
                };
                bool isExistingPhone = _context.Phones
                    .Any(x => x.PhoneNumber == vm.PhoneNumber);
                if (isExistingPhone)
                {
                    ModelState.AddModelError("PhoneNumber", "This Phone number is exist choise new");
                    return View();
                }
                _context.Contacts.Add(contact);
                await _context.SaveChangesAsync();
                return Redirect($"~/Contacts/Edit/{contact.Id}");
            }
            else
            {
                return View();
            }
        }

        public IActionResult Edit(Guid id)
        {
            //безотложно подгружаем множество связей для нашей модели представления
            //в более новых EF есть оптмизиации например .AsSplitQuery() разделяя один запрос на множество
            //чтобы избежать "декартового взрыва"
            var contact = _context.Contacts
                .Include(x => x.CallingHistories)
                .Include(x => x.Conferences)
                .ThenInclude(xx => xx.Conference)
                .Include(x => x.Phone)
                .Include(x => x.Profile)
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefault();
            if (contact != null)
            {
                return View(_mapper.Map<ContactModelView>(contact));
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ContactModelView editedVm)
        {
            if (ModelState.IsValid)
            {
                //безотложно подгружаем множество связей для нашей модели представления
                //в более новых EF есть оптмизиации например .AsSplitQuery() разделяя один запрос на множество
                //чтобы избежать "декартового взрыва"
                var contact = _context.Contacts
                    .Include(x => x.CallingHistories)
                    .Include(x => x.Conferences)
                    .Include(x => x.Phone)
                    .Include(x => x.Profile)
                    .AsNoTracking()
                    .Where(x => x.Id == editedVm.Id)
                    .FirstOrDefault();
                
                if (contact != null)
                {
                    var number = _context.Phones
                        .Where(x => x.PhoneNumber == editedVm.PhoneNumber 
                        && x.ContactId != editedVm.Id)
                        .FirstOrDefault();
                    if(number != null && !string.IsNullOrEmpty(number.PhoneNumber))
                    {
                        ModelState.AddModelError("Phone number", "This Phone number is exist choise new");
                        return View();
                    }
                    
                    contact.Phone.PhoneNumber = editedVm.PhoneNumber;
                    contact.Profile.DisplayName = editedVm.DisplayName;
                    _context.Contacts.Update(contact);
                    await _context.SaveChangesAsync();
                    return View(_mapper.Map<ContactModelView>(contact));
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return View();
            }
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            //безотложно подгружаем множество связей для нашей модели представления
            //в более новых EF есть оптмизиации например .AsSplitQuery() разделяя один запрос на множество
            //чтобы избежать "декартового взрыва"
            var contact = _context.Contacts
                .Include(x => x.Conferences)
                .Include(x => x.Phone)
                .Include(x => x.Profile)
                .Include(x => x.CallingHistories)
                .Where(x => x.Id == id)
                .FirstOrDefault();
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();
                return Redirect("~/Contacts/Index");
            }
            return Content("Something wrong");
        }

        [HttpPost]
        public async Task<IActionResult> Call(CallModelView callVm)
        {
            if (ModelState.IsValid)
            {
                //безотложно подгружаем множество связей для нашей модели представления
                //в более новых EF есть оптмизиации например .AsSplitQuery() разделяя один запрос на множество
                //чтобы избежать "декартового взрыва"
                var contact = _context.Contacts
                    .Include(x => x.CallingHistories)
                    .Include(x => x.Phone)
                    .Include(x => x.Profile)
                    .Where(x => x.Id == callVm.CallerId)
                    .FirstOrDefault();
                var calledContact = _context.Contacts
                    .Include(x => x.CallingHistories)
                    .Include(x => x.Phone)
                    .Include(x => x.Profile)
                    .Where(x => x.Phone.PhoneNumber == callVm.PhoneNumber)
                    .FirstOrDefault();
                if(calledContact == null)
                {
                    ModelState.AddModelError("PhoneNumber", "this phone number doesnt contain in db");
                    return PartialView("_CallForm", callVm);
                }
                if(callVm.PhoneNumber == contact.Phone.PhoneNumber)
                {
                    ModelState.AddModelError("PhoneNumber", "can't call yourself");
                    return PartialView("_CallForm", callVm);
                }
                if (contact != null)
                {
                    var callingHistoryForCaller = new CallingHistory
                    {
                        LogOwner = contact,
                        CallerName = contact.Profile.DisplayName,
                        CallerPhone = contact.Phone.PhoneNumber,
                        CalledPhone = calledContact.Phone.PhoneNumber,
                        CalledName = calledContact.Profile.DisplayName,
                        DateCall = DateTime.Now
                    };
                    var callingHistoryForCalled = new CallingHistory
                    {
                        LogOwner = calledContact,
                        CallerName = contact.Profile.DisplayName,
                        CallerPhone = contact.Phone.PhoneNumber,
                        CalledPhone = calledContact.Phone.PhoneNumber,
                        CalledName = calledContact.Profile.DisplayName,
                        DateCall = DateTime.Now
                    };
                    contact.CallingHistories.Add(callingHistoryForCaller);
                    calledContact.CallingHistories.Add(callingHistoryForCalled);
                    await _context.SaveChangesAsync();
                }
                ViewBag.Message = "Call complete";
                return Redirect($"~/Contacts/Edit/{callVm.CallerId}");
            }
            else
            {
                return PartialView("_CallForm", callVm);
            }
        }

        [HttpGet]
        public IActionResult GetDetailBilling(Guid id, DateTime StartRangeDate, DateTime EndRangeDate)
        {
            if(StartRangeDate == default)
            {
                ModelState.AddModelError("StartRangeDate", "Get another start date");
                return View(new ContactDetailBillingBetweenRangeModelView());
            }
            if(EndRangeDate == default || EndRangeDate < StartRangeDate)
            {
                ModelState.AddModelError("EndRangeDate", "Get another start date");
                return View(new ContactDetailBillingBetweenRangeModelView());
            }
            ViewData["StartRangeDate"] = StartRangeDate;
            ViewData["EndRangeDate"] = EndRangeDate;

            var contact = _context.Contacts
                .Include(x => x.CallingHistories)
                .Where(x => x.Id == id)
                .AsNoTracking();
            if(contact == null)
            {
                ModelState.AddModelError("ContactId", "Something wrong doesnt containt contact");
                return View(new ContactDetailBillingBetweenRangeModelView());
            }
            var resultList = contact.SelectMany(x => x.CallingHistories
                .Where(xx => xx.DateCall >= StartRangeDate && xx.DateCall <= EndRangeDate))
                .ToList();
            var complexContact = contact.FirstOrDefault();
            complexContact.CallingHistories = resultList;
            var mapedList = _mapper.Map<ContactDetailBillingBetweenRangeModelView>(complexContact);
            return View(mapedList);
        }
    }
}
