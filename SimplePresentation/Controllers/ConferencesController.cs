using AutoMapper;
using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimplePresentation.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplePresentation.Controllers
{
    public class ConferencesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ConferencesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var presentList = _context.Conferences.Include(x => x.Contacts);
            var mappedList = new List<ConferenceEditModelView>();
            foreach (var item in presentList)
            {
                mappedList.Add(_mapper.Map<ConferenceEditModelView>(item));
            }
            return View(mappedList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ConferenceModelView vm)
        {
            //можно написать атрибут или использовать другой подход
            if(vm.DateConference < DateTime.Now.AddMinutes(1))
            {
                ModelState.AddModelError("DateСonference", $"Date must be more than {DateTime.Now}");
                return View();
            }

            if (ModelState.IsValid)
            {
                var conference = new Conference
                {
                    DateConference = vm.DateConference,
                    Contacts = new List<ContactConference>()
                };
                foreach (var item in vm.Contacts)
                {
                 //безотложно подгружаем множество связей для нашей модели представления
                 //в более новых EF есть оптмизиации например .AsSplitQuery() разделяя один запрос на множество
                 //чтобы избежать "декартового взрыва"
                    var contact = _context.Contacts
                         .Include(x => x.Phone)
                         .Include(x => x.Profile)
                         .Include(x => x.Conferences)
                         .Where(x => x.Profile.DisplayName == item.DisplayName && x.Phone.PhoneNumber == item.PhoneNumber)
                         .FirstOrDefault();
                    if (contact != null)
                    {
                        conference.Contacts.Add(new ContactConference
                        {
                            Conference = conference,
                            Contact = contact
                        });
                    }
                    else
                    {
                        ModelState.AddModelError("Contact", $"Contact with this number doesnt contain in db");
                        return View();
                    }
                }
                _context.Conferences.Add(conference);
                await _context.SaveChangesAsync();
                return Redirect("~/Conferences/Index");
            }
            return View();
        }

        public IActionResult Edit(Guid id)
        {
            //безотложно подгружаем множество связей для нашей модели представления
            //в более новых EF есть оптмизиации например .AsSplitQuery() разделяя один запрос на множество
            //чтобы избежать "декартового взрыва"
            var conference = _context.Conferences
                .Include(x => x.Contacts)
                .ThenInclude(x => x.Contact)
                .ThenInclude(x => x.Phone)
                .Include(x => x.Contacts)
                .ThenInclude(x => x.Contact)
                .ThenInclude(x => x.Profile)
                .Where(x => x.Id == id)
                .AsNoTracking()
                .FirstOrDefault();
            var mappedList = new List<ConferenceEditModelView>();
            return View(_mapper.Map<ConferenceEditModelView>(conference));
        }

        [HttpPost]
        public IActionResult Edit(ConferenceEditModelView vm,  string deleteId)
        {
            if (ModelState.IsValid)
            {
                if (String.IsNullOrEmpty(deleteId))
                {
                    //безотложно подгружаем множество связей для нашей модели представления
                    //в более новых EF есть оптмизиации например .AsSplitQuery() разделяя один запрос на множество
                    //чтобы избежать "декартового взрыва"
                    var conference = _context.Conferences
                        .Include(x => x.Contacts)
                        .ThenInclude(x => x.Contact)
                        .ThenInclude(x => x.Phone)
                        .Include(x => x.Contacts)
                        .ThenInclude(x => x.Contact)
                        .ThenInclude(x => x.Profile)
                        .Where(x => x.Id == vm.ConferenceId)
                        .FirstOrDefault();
                    if (conference != null)
                    {
                        //огромное количество валидаций конференции которую надо провести перед сохранением
                    }
                }
                return Redirect($"~Conferences/Edit/{vm.ConferenceId}");
            }
            else
            {
                return View(vm);
            }
            
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var conference = _context.Conferences.Where(x => x.Id == id).FirstOrDefault();
            if(conference != null)
            {
                _context.Remove(conference);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
