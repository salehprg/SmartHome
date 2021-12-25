import { Component, Input, OnInit } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";
import { HomeService } from "src/app/modules/home/_services/home.service";

@Component({
  selector: 'sh-home-lockup',
  templateUrl: './home-lockup.component.html',
  styleUrls: ['./home-lockup.component.scss']
})

export class HomeLockupComponent implements OnInit{

  smartPlugs : any = []
  @Input() active: string;

  roomId: string;

  constructor(public homeService: HomeService , public translate : TranslateService) { }

  ngOnInit(){
    this.homeService.fetchSmartPlugs()
    this.homeService._smartPlugs$.subscribe(sp => this.smartPlugs = sp)
    if(this.homeService._selectedRoom$.value){
      this.roomId = this.homeService._selectedRoom$.value.roomId
    }
    this.homeService._selectedRoom$.subscribe(sr => {
      if (sr) this.roomId = sr.roomId
    })
  }

  getSp(id){
    const room = this.homeService._home$.value.rooms.find(room => room.roomId === id)
    return room.roomDevices.filter(m => m.deviceType === 4)
  }

  isEmpty(modules: any) {
    let empty = true;
    for (let module in modules) {
      if (modules[module].length > 0) empty = false;
    }
    return empty;
  }
}
