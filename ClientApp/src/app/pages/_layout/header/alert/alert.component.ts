import { AlertService } from './alert.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Component, HostListener, OnInit, ViewChild } from '@angular/core';

@Component({
  selector: 'sh-alert',
  templateUrl: './alert.component.html',
  styleUrls: ['./alert.component.scss'],
  providers: [NgbModal],
})
export class AlertComponent implements OnInit {
  @ViewChild('modal') m;

  constructor(public alert: AlertService, private modal: NgbModal) {

  }

  ngOnInit(){
    //

    if (!window.navigator.onLine) {
      this.alert.alert('offline', 'You\'ve gone offline! Check your internet Connection');
    }
  }

  show() {
    this.modal.open(this.m, {
      centered: true,
    });
  }

  @HostListener('window:offline')
  off() {
    this.alert.alert('offline', 'You\'ve gone offline! Check your internet Connection');
  }

  @HostListener('window:online')
  on() {
    this.alert.clear('offline');
  }
}
