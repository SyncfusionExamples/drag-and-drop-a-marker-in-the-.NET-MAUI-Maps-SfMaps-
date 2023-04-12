using Syncfusion.Maui.Maps;

namespace MarkerDragAndDrop
{
    public class MapsBehavior : Behavior<ContentPage>
    {
        private MapTileLayer layer;

        protected override void OnAttachedTo(ContentPage bindable)
        {
            base.OnAttachedTo(bindable);
            layer = bindable.FindByName<MapTileLayer>("layer");
            layer.Panning += Layer_Panning;
        }

        private void Layer_Panning(object sender, PanningEventArgs e)
        {
            var position = layer.GetLatLngFromPoint(e.Position);
            if (layer.ZoomPanBehavior == null)
            {
                layer.ZoomPanBehavior = new MapZoomPanBehavior()
                {
                    EnablePanning = false
                };
            }

            foreach (var marker in layer.Markers)
            {
                var halfWidth = marker.IconWidth / 2;
                var halfHeight = marker.IconHeight / 2;
                if (position.Latitude > (marker.Latitude - (halfWidth / layer.ZoomPanBehavior.ZoomLevel)) &&
                    position.Longitude > (marker.Longitude - (halfHeight / layer.ZoomPanBehavior.ZoomLevel)) &&
                    position.Latitude < (marker.Latitude + (halfWidth / layer.ZoomPanBehavior.ZoomLevel)) &&
                    position.Longitude < (marker.Longitude + (halfHeight / layer.ZoomPanBehavior.ZoomLevel)))
                {
                    marker.Latitude = position.Latitude;
                    marker.Longitude = position.Longitude;
                    break;
                }
            }
        }

        protected override void OnDetachingFrom(ContentPage bindable)
        {
            base.OnDetachingFrom(bindable);
            if (this.layer != null)
            {
                layer.Panning -= Layer_Panning;
            }

            this.layer = null;
        }
    }
}
