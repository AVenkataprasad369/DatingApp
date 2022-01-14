import { ChangeDetectionStrategy, Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Message } from 'src/app/_models/message';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {

  // @Input() username : string;
  @ViewChild('messageForm') messageForm: NgForm;
  @Input() messages: Message[];
  @Input() username: string;
  messageContent: string;
  
  // constructor(private messagesService: MessageService) { } // post signalR implementation
  constructor(public messagesService: MessageService) { }
  
   ngOnInit(): void {  
  }

  sendMessage(){
    // this.messagesService.sendMessage(this.username, this.messageContent).subscribe(message => {
    //   this.messages.push(message);
    //   this.messageForm.reset();
    // })

    this.messagesService.sendMessage(this.username, this.messageContent).then(() => {
      this.messageForm.reset();
    })
  }

  // ngOnInit(private messageService: MessageService): void {
  //    this.loadMessages();
  // }

  //// below method moved to member detail component
  // loadMessages(){
  //   this.messageService.getMessageThread(this.username).subscribe(messages => {
  //     this.messages = messages;
  //   })
  // }

}
