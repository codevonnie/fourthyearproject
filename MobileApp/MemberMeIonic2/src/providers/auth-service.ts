import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import 'rxjs/add/operator/map';
import 'rxjs';

/*
  Generated class for the AuthService provider.

  See https://angular.io/docs/ts/latest/guide/dependency-injection.html
  for more info on providers and Angular 2 DI.
*/
@Injectable()
export class AuthService {

    accessToken: "YXBwNTc5NzU5MDAtYUVnQXRYOnRHbTZGd09LZ1U3c1F5UERVQUNq";
    
    

  constructor(public http: Http) {
    console.log('Hello AuthService Provider');
  }

  login(){
    let headers = new Headers();
    var URL = "http://localhost:8080/api/authenticate";
    var body = JSON.stringify({
    email: "spotts@email.com",
    password: "mypass"
    });
    headers.append('Authorization', 'Bearer ' + this.accessToken);
    headers.append('Content-Type', 'application/json');
 
    this.http.post(URL, body, {headers: headers})
    .map(res => res.json());
 
  }
}
