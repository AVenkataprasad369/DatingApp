import { Component, OnInit, ViewChild } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { NgxGalleryImage } from '@kolkov/ngx-gallery';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { MessageService } from 'src/app/_services/message.service';
import { Message } from 'src/app/_models/message';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
@ViewChild('memberTabs', {static: true}) memberTabs: TabsetComponent;
member: Member;
galleryOptions: NgxGalleryOptions[];
galleryImages: NgxGalleryImage[];
activeTab: TabDirective;
messages: Message[] = [];

  constructor(private memberService: MembersService,
              private route: ActivatedRoute,
              private messageService: MessageService) { }

  ngOnInit(): void {

    this.route.data.subscribe(data => {
      this.member = data.member;
    })
    //// Before adding above code (resolver concept)
    // this.loadMember();
    
    this.route.queryParams.subscribe(params => {
      params.tab ? this.selectTab(params.tab) : this.selectTab(0);
    })

    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false
      }
    ]

    // Below code copied from loadMember() method, because we no more need this method due to resolver concept
    this.galleryImages = this.getImages();
  }

  getImages(): NgxGalleryImage[] {
    const imageUrls = [];
    for(const photo of this.member.photos){
      imageUrls.push({
          small: photo?.url,
          medium: photo?.url,
          big: photo?.url
      })
    }

    return  imageUrls;

  }

  //// We have commented this method, because we implemented resolver functionality
  // loadMember(){
  //   this.memberService.getMember(this.route.snapshot.paramMap.get('username'))
  //   .subscribe(member => {
  //     this.member = member;
  //     this.galleryImages = this.getImages();
  //   });    
  //   console.log(`options ${this.galleryOptions}`);
  //   console.log(`images ${this.galleryImages}`);
  // }

  loadMessages(){
    this.messageService.getMessageThread(this.member.username).subscribe(messages => {
      this.messages = messages;
    })
  } 

  selectTab(tabId: number){
      this.memberTabs.tabs[tabId].active = true;
  }

  onTabActivated(data: TabDirective){
    this.activeTab = data;
    if(this.activeTab.heading === 'Messages' && this.messages.length === 0){
      this.loadMessages();
    }
  }
  
}
