import { environment } from './../../environments/environment.prod';
import { Injectable, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root',
})
export class SocketService {
  connection: HubConnection;

  constructor() {
    this.connection = new HubConnectionBuilder()
    .withUrl(`${environment.baseUrl}/socket`)
    .build();

    this.connection.start();
  }
}
