import { DomSanitizer } from '@angular/platform-browser';
import { LayoutService } from 'src/app/pages/_layout/layout.service';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'energy-widget',
  templateUrl: './energy-widget.component.html',
})
export class EnergyWidget implements OnInit {
  total: number;
  price: number;
  mostUsage: string;
  hasError: boolean;
  isLoading: boolean;

  constructor(
    private http: HttpClient,
    private layout: LayoutService,
    private sanitizer: DomSanitizer
  ) {}

  ngOnInit() {
    this.hasError = false;
    this.isLoading = true;
    this.http
      .post(`${environment.apiUrl}/Smartplug`, {
        format: '2m',
        fromDate: '',
        toDate: '',
      })
      .subscribe(
        (res: any) => {
          //
          this.total = Math.floor(res.total);
          this.price = Math.floor(res.totalPrice);
          let max = -1;
          let mostUsageSmartPlug;

          if (res.outputs.length > 0) {
            res.outputs.forEach((o) => {
              if (o.totalWatt >= max) {
                mostUsageSmartPlug = o;
                max = o.totalWatt;
              }
            });

            //

            this.mostUsage = mostUsageSmartPlug.baseDevice.name;
          }
          this.hasError = false;

          // setTimeout(()=>{this.isLoading = false} , 1000)
          this.isLoading = false;
        },
        (err) => {

          this.hasError = true;
          this.isLoading = false;
        }
      );
  }

  getBG() {
    const theme = this.layout.getThemeName();
    if (theme === 'saga-blue') return this.sanitizer.bypassSecurityTrustStyle("url('./assets/media/bg/bg-7.jpg')");
    else if (theme === 'arya-blue') return this.sanitizer.bypassSecurityTrustStyle("url('./assets/media/bg/bg-4.jpg')");
    else return this.sanitizer.bypassSecurityTrustStyle("url('./assets/media/bg/bg-5.jpg')");
  }
}
