import { Component, Input, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { NgbAccordion, NgbAccordionConfig, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { WifiService } from "../_services/wifi.service";

@Component({
    selector: 'sh-wifi-card',
    templateUrl: './wifi-card.component.html',
    styleUrls: ['./wifi-card.component.scss'],
    providers: [NgbAccordionConfig]
})

export class WifiCardComponent implements OnInit {
    @Input() name: string;
    @Input() isConnected: boolean;
    @Input() id: number;


    ngOnInit() {
        // if (this.wifiService.connectedWifi) {
        //
        //
        //

        //     if (this.name === this.wifiService.connectedWifi.id) {
        //         this.isConnected = true
        //     }
        //     else {
        //         this.isConnected = false
        //     }
        // }
        // //

    }

    constructor(
        private router: Router,
        private modalService: NgbModal,
        private wifiService: WifiService) {

    }

    goToPassword() {
        this.modalService.dismissAll()
        this.wifiService.setWifiName(this.name)
        this.router.navigateByUrl(`/wifi/${this.id}`)
    }

    disconnect(){
        this.wifiService.disconnect()
        this.modalService.dismissAll()
    }
}
