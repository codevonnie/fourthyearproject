import { Injectable } from '@angular/core';
import {Http, Response} from "@angular/http";
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import {Observable} from "rxjs";

@Injectable()
export class ProfileService {

  api_url: string = 'https://restapicust.herokuapp.com/api/businessmembers';

  constructor(public http: Http) {
    console.log('Hello Profileservice Provider');
  }

  public getBusinessMembers(){
    return this.http.get(this.api_url)
    .map(res => res.json())
  }

}


