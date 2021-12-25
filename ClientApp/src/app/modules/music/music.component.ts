import { SocketService } from './../../core/socket.service';
import { Component } from '@angular/core';

@Component({
  selector: 'sh-music',
  templateUrl: './music.component.html',
  styleUrls: ['./music.component.scss'],
})
export class MusicComponent {

  constructor(public socket: SocketService) {
    this.socket.connection.on('trackname', (from, body) => {
      console.log('track name', from, body);
    })

    this.socket.connection.on('trackstatus', (from, body) => {
      console.log('track status', from, body);
    })

    this.socket.connection.on('trackposition', (from, body) => {
      console.log('track position', from, body);
    })
  }

}
