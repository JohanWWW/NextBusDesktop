# NextBusDesktop
NextBus is a Windows 10 application in which the user can plan their trips and search for departure board for a specific stop.

## Departure Board
The departure board view is designed to be left open as it is polling data from Västtrafik's servers every minute, keeping the departure board up to date.
A slim pulsating orange horizontal line appears below the search box whenever data is requested from the server and fades away when done.

### Track Filter
The departure list can be filtered to only show departures departing from a specified track. The list is unfiltered by default.
If a stop is missing track numbers or it is unknown, the track number defaults to "-" which means missing/unknown.

### Rescheduled Departure
Should the departure time change a yellow pulsing indicator will appear on the departure.
In case of a late departure a "+" sign will appear to the right of scheduled time together with amount of minutes after schedule a departure is expected.
In case of an early departure a "-" sign will appear to the right of scheduled time together with amount of minutes before schedule a departure is expected.
Values for remaining time until departure automatically adjust in case of a reschedule, in other words minutes are added on late departures and subtracted on earlier departures.

## Trip Planner
The trip planner is currently a work in progress and contains bugs. With the trip planner tool, the user can plan their trips between two points. 
A trip provides the user with a step-by-step guide showing which lines and vehicles to take and how to get to their connecting departure.
If a line within a trip is rescheduled, this is presented to the user in a human readable language.

## Lanugage Support
NextBus currently supports following languages:
- English (US)
- Swedish

However the application works best in Swedish due to some information on the servers being based in Swedish.
Language settings can be accessed in settings on the side bar.
