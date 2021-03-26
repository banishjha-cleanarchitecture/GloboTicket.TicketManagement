using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Infrastructure;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventsExport
{
    public class GetEventExportQueryHandler : IRequestHandler<GetEventExportQuery, EventExportFileVm>
    {
        private readonly IAsyncRepository<Event> _asyncRepository;
        private readonly IMapper _mapper;
        private readonly ICsvExporter _csvExporter;

        public GetEventExportQueryHandler(IAsyncRepository<Event> asyncRepository , IMapper mapper , ICsvExporter csvExporter)
        {
            _asyncRepository = asyncRepository;
            _mapper = mapper;
            _csvExporter = csvExporter;
        }

        public async Task<EventExportFileVm> Handle(GetEventExportQuery request, CancellationToken cancellationToken)
        {
            var allEvents = _mapper.Map<List<EventExportDto>>((await _asyncRepository.ListAllAsync()).OrderBy(x => x.Date));

            var fileData = _csvExporter.ExportEventsToCsv(allEvents);

            var eventExportFileDto = new EventExportFileVm { EventExportFileName = $"{Guid.NewGuid()}.csv", ContentType = "text/csv", Data = fileData };
            
            return eventExportFileDto;
        }
    }
}
