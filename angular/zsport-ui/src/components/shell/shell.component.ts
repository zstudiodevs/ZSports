import { CommonModule } from "@angular/common";
import { Component, input, OnInit } from "@angular/core";
import { MatToolbarModule } from "@angular/material/toolbar";
import { MatIconModule } from "@angular/material/icon";
import { MatButtonModule } from "@angular/material/button";
import { MatSidenavModule } from "@angular/material/sidenav"
import { BreakpointObserver } from "@angular/cdk/layout";

@Component({
    selector: "zs-shell",
    templateUrl: "./shell.component.html",
    styleUrl: "./shell.component.scss",
    imports: [
        CommonModule,
        MatToolbarModule,
        MatIconModule,
        MatButtonModule,
        MatSidenavModule
    ]
})
export class ShellComponent implements OnInit {
    sidenavMode : "side" | "over" = "side";
    isSidenavOpened = true;
    isDarkTheme = true;

    profileAction = input.required<() => void>();
    constructor(private breakpointObserver: BreakpointObserver) {}

    ngOnInit() {
        this.breakpointObserver.observe(['(max-width: 600px)']).subscribe(result => {
            this.sidenavMode = result.matches ? 'over' : 'side';
            this.isSidenavOpened = !result.matches;
        });
    }
    toggleTheme() {
        this.isDarkTheme = !this.isDarkTheme;
        document.body.classList.toggle('dark-theme');
    }
    onProfileClick() {
        if (this.profileAction) {
            this.profileAction();
        }
    }
}