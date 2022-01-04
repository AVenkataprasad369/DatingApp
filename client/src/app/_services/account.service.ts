import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { map } from 'rxjs/operators';
import { ReplaySubject } from 'rxjs';
import { User } from '../_models/user';
import { environment } from 'src/environments/environment';
import { PresenceService } from './presence.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseUrl = environment.apiUrl; //removed the hard coded value, "https://localhost:5001/api/";
  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ =  this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private presence: PresenceService) { }
  
  login(model: any){
    return this.http.post(this.baseUrl+ "account/login",model).pipe(
      map((response: any) => {
        const user= response;

        if(user){ 
          this.setCurrentUser(user);
          this.presence.createHubConnection(user);
        }
      })
    );
  }

  register(model: any){
   return this.http.post(this.baseUrl+ 'account/register',model).pipe(
     map((user: User) => {
       if(user){
         this.setCurrentUser(user);
         this.presence.createHubConnection(user);
       }
     })
   );
  }


  setCurrentUser(user: User){
    user.roles = [];
    const roles = this.getDecodedToken(user.token).role;
    Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);
    localStorage.setItem('user',JSON.stringify(user));
    // console.log(JSON.parse(localStorage.getItem('user')).token)
    this.currentUserSource.next(user);
  }
  
  logout(){
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
    this.presence.stopHubConnection();
  }

  getDecodedToken(token){
    return JSON.parse(atob(token.split('.')[1]));
  }

}
