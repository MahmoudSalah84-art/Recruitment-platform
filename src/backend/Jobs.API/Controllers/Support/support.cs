using CloudinaryDotNet;
using Jobs.API.Controllers.Support;

namespace Jobs.API.Controllers.Support
{
	public class support
	{
	}
}






//GET / api / support / tickets                → GetAllTicketsQuery(paginated + filtered)
//GET / api / support / tickets /{ ticketId}     → GetTicketDetailsQuery
//POST   /api/support/tickets/{ticketId}/ reply → ReplyToTicketCommand
//PATCH  /api/support/tickets/{ticketId}/ close  → CloseTicketCommand
//PATCH  /api/support/tickets/{ticketId}/ assign → AssignTicketCommand




//GET / api / support / reports                → GetAllReportsQuery
//PATCH  /api/support/reports/{reportId}/ resolve → ResolveReportCommand
//PATCH  /api/support/reports/{reportId}/ dismiss → DismissReportCommand




//GET    /api/support/faqs                   → GetAllFAQsQuery
//POST   /api/support/faqs                   → CreateFAQCommand
//PUT    /api/support/faqs/{faqId}           → UpdateFAQCommand
//DELETE /api/support/faqs/{faqId}           → DeleteFAQCommand