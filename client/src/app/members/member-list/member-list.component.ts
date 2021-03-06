import { Component, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';
import { Observable } from 'rxjs'
import { Pagination } from 'src/app/_models/pagination';
import { UserParams } from 'src/app/_models/userParams';
import { AccountService } from 'src/app/_services/account.service';
import { take } from 'rxjs/operators';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  //// Below Caching implementation turned off to implement pagination 
// members$: Observable<Member[]>;
   members: Member[];
   pagination: Pagination;
   userParams: UserParams;
  //  user: User; // facing error with this line, so to solution added below line
   user: any;
   genderList = [{value:'male', display:'Males'}, {value:'female', display:'Females'}];

  constructor(private memberService : MembersService) { 
     this.userParams = this.memberService.getUserParams();
  }

  ngOnInit(): void {

    //// Below Caching implementation turned off to implement pagination 
    // this.members$ = this.memberService.getMembers();
    
    this.loadMembers();

  }

 loadMembers() {   
    this.memberService.setUserParams(this.userParams);
    this.memberService.getMembers(this.userParams).subscribe(response => {
      this.members = response.result;
      this.pagination = response.pagination;
    })
  }

  resetFilters(){
    this.userParams = this.memberService.resetUserParams();
    this.loadMembers();
  }

  pageChanged(event: any){
    this.userParams.pageNumber = event.page;
    this.memberService.setUserParams(this.userParams);
    this.loadMembers();
  }

}
