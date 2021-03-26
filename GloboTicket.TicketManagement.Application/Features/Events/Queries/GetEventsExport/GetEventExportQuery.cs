using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventsExport
{
    public class GetEventExportQuery : IRequest<EventExportFileVm>
    {
    }
}
