using GloboTicket.TicketManagement.Application.Features.Events.Commands.CreateEvent;
using GloboTicket.TicketManagement.Application.Features.Events.Commands.DeleteEvent;
using GloboTicket.TicketManagement.Application.Features.Events.Commands.UpdateEvent;
using GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventDetail;
using GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventsExport;
using GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventsList;
using GloboTicket.TicketManagement.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EventsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("all",Name = "GetAllEvents")]
        public async Task<ActionResult<List<EventListVm>>> GetAllEvents()
        {
            var result = await _mediator.Send(new GetEventsListQuery());
            return Ok(result);
        }

        [HttpGet("eventdetail", Name = "GetEventDetail")]
        public async Task<ActionResult<EventDetailVm>> GetEventDetail(Guid id)
        {
            GetEventDetailQuery getEventDetail = new GetEventDetailQuery { Id = id };
            var result = await _mediator.Send(getEventDetail);

            return Ok(result);
        }

        [HttpPost("AddEvent", Name = "CreateEvent")]
        public async Task<ActionResult<BaseResponse>> Create([FromBody] CreateEventCommand createEventCommand)
        {
            var response = await _mediator.Send(createEventCommand);
            return Ok(response);
        }

        [HttpPut( Name = "UpdateEvent")]
        public async Task<ActionResult> Update([FromBody] UpdateEventCommand updateEventCommand)
        {
             await _mediator.Send(updateEventCommand);
            return NoContent();
        }

        [HttpDelete("{id}" , Name = "DeleteEvent")]
        public async Task<ActionResult> Delete(Guid Id)
        {
            var deleteCommand = new DeleteEventCommand { EventId = Id };
            await _mediator.Send(deleteCommand);
            return NoContent();
        }

        [HttpGet("export",Name = "EventsExport")]
        public async Task<FileResult> ExportEvents()
        {
            var fileDto = await _mediator.Send(new GetEventExportQuery());
            return File(fileDto.Data, fileDto.ContentType, fileDto.EventExportFileName);
        }

    }
}
