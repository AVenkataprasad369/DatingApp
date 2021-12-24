using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;
using API.DTOs;
using API.Helpers;
using API.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class MessageRepository: IMessageRepository
    { 
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public MessageRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);

        }
        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<Message> GetMessage(int id)
        {
            //// This code commented because error in Messsage controller ( at line 85)
            // return await _context.Messages.FindAsync(id); 
            return await _context.Messages
                    .Include(u => u.Sender)
                    .Include(u => u.Recipient)
                    .SingleOrDefaultAsync(x => x.Id == id); 
        }

        public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            var query = _context.Messages
                        .OrderByDescending(m => m.MessageSent)
                        .AsQueryable();
            
            query = messageParams.Container switch 
            {
                "Inbox" => query.Where(u => u.Recipient.UserName == messageParams.Username 
                        && u.RecipientDeleted == false),
                "Outbox" => query.Where(u => u.Sender.UserName == messageParams.Username 
                        && u.SenderDeleted == false),
                _ => query.Where(u => u.Recipient.UserName == messageParams.Username
                                 && u.RecipientDeleted == false && u.DateRead == null)              
            };

            var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);

            return await PagedList<MessageDto>.CreateAsync(messages,
                     messageParams.PageNumber, messageParams.PageSize);

        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, 
                string recipientUsername)
        {
            // Get the conversation of the users
           var messages = await _context.Messages
            .Include(u => u.Sender).ThenInclude(p => p.Photos)
            .Include(u => u.Recipient).ThenInclude(p => p.Photos)
            .Where(m => m.Recipient.UserName == currentUsername && m.RecipientDeleted == false
                        && m.Sender.UserName == recipientUsername
                        || m.Recipient.UserName == recipientUsername
                        && m.Sender.UserName == currentUsername && m.SenderDeleted == false
                        )
                        .OrderBy(m => m.MessageSent)
                        .ToListAsync();

            // Find the unread messages, which recieved for current user
            var unreadMessages = messages.Where(m => m.DateRead == null
             && m.Recipient.UserName == currentUsername).ToList();

            // If any unread messages, then mark them as red
             if(unreadMessages.Any())
             {
                 foreach(var message in unreadMessages)
                 {
                     message.DateRead = DateTime.Now;
                 }
                 await _context.SaveChangesAsync();
             }

             return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}