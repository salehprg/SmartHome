import { SocketService } from './../../../core/socket.service';
import { ChangeDetectorRef } from '@angular/core';
import { NotifierService } from 'angular-notifier';
import { HttpClient } from '@angular/common/http';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Component, ViewChild, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment.prod';

@Component({
  selector: 'sh-bluetooth-pair',
  templateUrl: './bluetooth-pair.component.html'
})
export class BluetoothPairComponent {
  bluetoothName: string = '';
  @ViewChild('bluetoothPair', { static: true }) modal;

  constructor(
    private modalService: NgbModal,
    private socket: SocketService,
    private http: HttpClient,
    private notif: NotifierService
    ) {
      this.socket.connection.on('pairBluetooth', (bluetoothName) => {
        this.bluetoothName = bluetoothName;
        modalService.open(this.modal);
      });
  }

  pair() {
    this.http.get(`${environment.apiUrl}/Bluetooth`).subscribe(
      res => {
        this.modalService.dismissAll();
        this.notif.show({
          type: 'success',
          message: 'Connected to' + this.bluetoothName + ' successfully'
        })
      },
      err => {

      }
    );
  }

  cancel() {
    this.modalService.dismissAll();
  }
}
