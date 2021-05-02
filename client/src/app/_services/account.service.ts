import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { map } from 'rxjs/operators';
import { ReplaySubject } from 'rxjs';
import { User } from '../_models/user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseUrl = environment.apiUrl; //removed the hard coded value, "https://localhost:5001/api/";
  private currentUserSource = new ReplaySubject(1);
  currentUser$ =  this.currentUserSource.asObservable();

  constructor(private http: HttpClient) { }
  
  login(model: any){
    return this.http.post(this.baseUrl+ "account/login",model).pipe(
      map((response: any) => {
        const user= response;

        if(user){          
          localStorage.setItem('user',JSON.stringify(user));
          console.log(JSON.parse(localStorage.getItem('user')).token)
          this.setCurrentUser(user);
        }
      })
    );
  }

  register(model: any){
   return this.http.post(this.baseUrl+ 'account/register',model).pipe(
     map(user => {
       if(user){
         localStorage.setItem('user',JSON.stringify(user));
         this.currentUserSource.next(user);
       }
     })
   );
  }

  setCurrentUser(user: User){
    this.currentUserSource.next(user);
  }
  
  logout(){
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }

}
